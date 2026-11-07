# Onshape Palette Studio v0.1

一个无需 API Key、无需浏览器注入的 **Onshape 原生 FeatureScript 配色工具**。

**本版本能力：**12 套色系 × 每套 12 个颜色 × 4 种分配模式 × 8 个可复现的变化配置；可以一次给当前 Part Studio 中的所有实心零件，或所选零件批量上色。改变该 Feature 的参数，就能重新给零件分配协调色。仅改变外观，不修改模型几何、材料、质量属性或配合关系。

## 安装：在 Onshape 中（不是在本地运行 `.fs` 文件）

1. 建议先在**副本/测试文档**中试用。打开包含至少 3 个独立实心零件的 Onshape 文档。
2. 点击左下角 `+` 添加 **Create Feature Studio**，新建一个 Feature Studio。
3. 将 `src/palette_studio.fs` 的**完整内容**粘贴进 Feature Studio，替换初始模板，保存/提交（Onshape 编辑器会做语法和预处理检查）。
4. 在 Part Studio 中，使用 Onshape **Add custom features** 将该 Feature Studio 的 `Palette Studio` 加到工具栏（同文档时可从自定义特征来源中查找）。
5. 建议把 Part Studio 的回滚条拖到最后、把该 Feature 放在建模特征列表末尾。点击 **Palette Studio**，选择 Palette / Color scope / Arrangement / Variation，确认。
6. 若装配体引用这些零件，返回 Assembly 查看外观变化。可以编辑同一个 Feature 来更换色系或排列，无需逐一选色。

**选择范围：** `All solid parts` 默认将当前 Part Studio 里所有实心零件上色。选 `Selected parts only` 可以只改变选中的零件。

**撤销/复原：**撤销插入该 Feature，或在特征树中删除/抑制它，返回先前的外观历史。如果原先对部分面单独指定了外观，面级外观可能覆盖零件级配色。

## 颜色预览

双击 `preview/palette-preview.html`，无需网络或账号即可预览 12 色系、查看 12 个零件的示意组合、切换排列、复制 HEX、导出 CSV。预览页**不连接 Onshape**，页面的色系/排列/变化选项和 FeatureScript 相同。

## 特别说明：装配体的支持边界

- 此版本通过修改**来源 Part Studio** 的零件 Appearance，让同样的颜色显示在引用该零件的 Assemblies 中。
- 如果你的 Assembly 引用了多个不同 Part Studio/外部文档，需要到各来源 Part Studio 使用一次本工具；不能从一个装配体直接修改所有零件的源文件。
- 同一源零件在同一配置下被装入多个位置时，其外观通常相同；不能靠此 Feature 给每个 instance 单独换色。
- 只读/版本引用、导入的供应商零件、外观配置和面级覆盖，可能影响最终能否改变或显示预期颜色。
- 此包尚**未通过你的 Onshape 账户实际编译/装配模型验证**，提供源码、纯离线检查和可复现的安装说明。若 Onshape Feature Studio 的编译器提示具体报错，可据报错修正。
- 若需要**真正的装配体页面一键改色、包括不同来源的零件**，需要另做一个 Onshape OAuth App（托管后端 + 权限授权 + 按来源文档/配置遍历并更新），不能由纯 FeatureScript 在 Assembly 中直接完成。

## 项目文件

- `src/palette_studio.fs` — 在 Onshape 内安装和运行的真实 FeatureScript 代码。
- `palettes.json` — 12 套色系，144 个 HEX 色值（配色源数据）。
- `preview/palette-preview.html` — 离线可视化预览页面。
- `tests/test_palettes.py` — 不依赖 Onshape 的静态检查；运行 `python -m unittest discover -s tests -v`。
- `build.py` — 重生成 FeatureScript 的辅助脚本，方便自定义色表。

## 技术参考

- 官方 FeatureScript 文档：https://cad.onshape.com/FsDoc/index.html
- 官方 Custom Feature：https://www.onshape.com/en/features/custom-features
- 官方 Appearance：https://cad.onshape.com/help/Content/PartStudio/customizing_appearances.htm

License: MIT. This project is a community prototype, not an official PTC/Onshape product.