const $ = id => document.getElementById(id);
let plan=null;
const params=new URLSearchParams(location.search);
if(params.has('documentId') && params.has('workspaceOrVersionId') && params.has('tabElementId')){
  const did=params.get('documentId'), wvm=params.get('workspaceOrVersion')||'w', wvmid=params.get('workspaceOrVersionId'), eid=params.get('tabElementId');
  $('url').value=`https://cad.onshape.com/documents/${did}/${wvm}/${wvmid}/e/${eid}`;
}
$('variation').innerHTML=Array.from({length:8},(_,i)=>`<option value="${i}">V${i+1}</option>`).join('');
function say(text,isError=false){$('status').textContent=text;$('status').style.color=isError?'#ffa3a3':'#a6efdd';}
async function api(path){const res=await fetch(path,{credentials:'same-origin'});const data=await res.json();if(!res.ok)throw Error(data.error||`HTTP ${res.status}`);return data;}
async function init(){const p=await api('/api/palettes');$('theme').replaceChildren(...Object.entries(p).map(([id,entry])=>{const el=document.createElement('option');el.value=id;el.textContent=entry.name;return el;}));if(p.MONOCHROME)$('theme').value='MONOCHROME';const s=await api('/api/status');say(s.authenticated?'Onshape 已连接：可读取装配体。':s.configured?'可连接 Onshape，或者先加载示例。':'OAuth 未配置：可先使用离线示例，真实读取需要本地环境变量。');}
function options(){const q=new URLSearchParams({theme:$('theme').value, arrangement:$('arrangement').value, variation:$('variation').value});return q;}
function textItem(label,detail){const li=document.createElement('li');li.textContent=detail?`${label}: ${detail}`:label;return li;}
function render(data){plan=data;$('results').hidden=false;$('counts').textContent=`${data.occurrenceCount} 个实例 · ${data.uniqueSourceCount} 种来源零件`;
 $('swatches').replaceChildren(...data.groups.map(g=>{const card=document.createElement('div');card.className='part';const swatch=document.createElement('div');swatch.className='chip';swatch.style.backgroundColor=g.color;const body=document.createElement('div');const name=document.createElement('strong');name.textContent=g.name;const shade=document.createElement('code');shade.textContent=g.color;const details=document.createElement('small');details.textContent=`${g.occurrences.length} 个实例 · ${g.documentId}/${g.elementId}/${g.partId}`;body.append(name,shade,details);card.append(swatch,body);return card;}));
 $('warnings').replaceChildren(...data.warnings.map(t=>textItem(t)));$('skipped').replaceChildren(...data.skipped.map(s=>textItem(s.path.join(' / '),s.reason)));
 say(`已生成 ${data.themeName} 配色计划（只读，没有修改 Onshape）。`);
}
async function run(demo){try{say('正在读取并计算配色…');const q=options();if(!demo)q.set('url',$('url').value);render(await api((demo?'/api/demo?':'/api/plan?')+q.toString()));}catch(err){say(err.message,true)}}
$('demo').addEventListener('click',()=>run(true));
$('palette-form').addEventListener('submit',e=>{e.preventDefault();run(false)});
$('logout').addEventListener('click',async()=>{await fetch('/auth/logout',{method:'POST'});say('已清除当前本地会话。')});
function save(content,name,type){const blob=new Blob([content],{type});const a=document.createElement('a');a.href=URL.createObjectURL(blob);a.download=name;a.click();setTimeout(()=>URL.revokeObjectURL(a.href),1000);}
$('json').addEventListener('click',()=>{if(plan)save(JSON.stringify(plan,null,2),'assembly-palette-plan.json','application/json')});
function csvField(x){return '"'+String(x??'').replaceAll('"','""')+'"';}
$('csv').addEventListener('click',()=>{if(!plan)return;const lines=[['Name','Color','Source Document','Source Element','Part ID','Configuration','Occurrences'].map(csvField).join(',')];for(const g of plan.groups)lines.push([g.name,g.color,g.documentId,g.elementId,g.partId,g.configuration,g.occurrences.length].map(csvField).join(','));save(lines.join('\r\n'),'assembly-palette-plan.csv','text/csv;charset=utf-8')});
init().catch(e=>say(e.message,true));
