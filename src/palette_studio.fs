// Onshape Palette Studio v0.2 — native FeatureScript custom feature.
// Apply to source Part Studios; appearances flow through to assemblies.
// FeatureScript 2543 is a conservative, previously used language version.
FeatureScript 2543;
import(path : "onshape/std/geometry.fs", version : "2543.0");

export enum PaletteTheme
{
    annotation { "Name" : "Ocean / 海洋蓝" }
    OCEAN,
    annotation { "Name" : "Forest / 森林绿" }
    FOREST,
    annotation { "Name" : "Sunset / 日落暖色" }
    SUNSET,
    annotation { "Name" : "Lavender / 薰衣草" }
    LAVENDER,
    annotation { "Name" : "Rose / 玫瑰粉" }
    ROSE,
    annotation { "Name" : "Earth / 大地莫兰迪" }
    EARTH,
    annotation { "Name" : "Graphite / 科技灰" }
    GRAPHITE,
    annotation { "Name" : "Pastel / 柔和混色" }
    PASTEL,
    annotation { "Name" : "Ice / 冰川冷色" }
    ICE,
    annotation { "Name" : "Copper / 铜金属" }
    COPPER,
    annotation { "Name" : "Neon / 霓虹科技" }
    NEON,
    annotation { "Name" : "Distinct / 清晰区分" }
    CONTRAST,
    annotation { "Name" : "Monochrome / 经典黑白" }
    MONOCHROME,
    annotation { "Name" : "Ivory Gold / 象牙白金" }
    IVORY_GOLD,
    annotation { "Name" : "Black Gold / 冠军黑金" }
    BLACK_GOLD,
    annotation { "Name" : "Stealth Black / 隐形战机" }
    STEALTH_BLACK,
    annotation { "Name" : "Black Red / 竞速红黑" }
    BLACK_RED,
    annotation { "Name" : "White Teal / 赛博白青" }
    WHITE_TEAL,
    annotation { "Name" : "Tidal Teal / 潮汐黑青" }
    TIDAL_TEAL,
    annotation { "Name" : "White Red Gold / 经典白金红" }
    WHITE_RED_GOLD,
    annotation { "Name" : "Violet Circuit / 紫色电路" }
    VIOLET_CIRCUIT,
    annotation { "Name" : "Blue Aluminum / 蓝银战队" }
    BLUE_ALUMINUM,
    annotation { "Name" : "Raw Aluminum / 原色铝合金" }
    RAW_ALUMINUM,
    annotation { "Name" : "White Carbon / 白色碳纤维" }
    WHITE_CARBON,
    annotation { "Name" : "Factory Orange / 工业橙灰" }
    ORANGE_GRAPHITE,
    annotation { "Name" : "Factory Yellow / 工业警示黄" }
    SAFETY_YELLOW,
    annotation { "Name" : "Cobot White / 协作机械白" }
    COBOT_WHITE,
    annotation { "Name" : "Silver Blue / 协作银蓝" }
    COBOT_SILVER_BLUE,
    annotation { "Name" : "Field Yellow / 户外机械黄" }
    SPOT_YELLOW,
    annotation { "Name" : "Titanium Carbon / 钛灰碳黑" }
    TITANIUM_CARBON
}

export enum PaletteScope
{
    annotation { "Name" : "All solid parts" }
    ALL,
    annotation { "Name" : "Selected parts only" }
    SELECTED
}

export enum PaletteArrangement
{
    annotation { "Name" : "Designed order" }
    GRADIENT,
    annotation { "Name" : "Alternating tones" }
    ALTERNATE,
    annotation { "Name" : "Spread out tones" }
    SPREAD,
    annotation { "Name" : "Reverse gradient" }
    REVERSE
}

export enum PaletteVariation
{
    annotation { "Name" : "Variation 1" }
    V1,
    annotation { "Name" : "Variation 2" }
    V2,
    annotation { "Name" : "Variation 3" }
    V3,
    annotation { "Name" : "Variation 4" }
    V4,
    annotation { "Name" : "Variation 5" }
    V5,
    annotation { "Name" : "Variation 6" }
    V6,
    annotation { "Name" : "Variation 7" }
    V7,
    annotation { "Name" : "Variation 8" }
    V8
}

function paletteColors(theme is PaletteTheme) returns array
{
    if (theme == PaletteTheme.OCEAN)
        return [
            color(0.070588, 0.227451, 0.356863), // #123A5B
            color(0.090196, 0.376471, 0.470588), // #176078
            color(0.105882, 0.505882, 0.596078), // #1B8198
            color(0.207843, 0.654902, 0.713725), // #35A7B6
            color(0.423529, 0.768627, 0.772549), // #6CC4C5
            color(0.643137, 0.866667, 0.854902), // #A4DDDA
            color(0.141176, 0.352941, 0.568627), // #245A91
            color(0.176471, 0.525490, 0.694118), // #2D86B1
            color(0.321569, 0.647059, 0.819608), // #52A5D1
            color(0.549020, 0.788235, 0.886275), // #8CC9E2
            color(0.098039, 0.423529, 0.478431), // #196C7A
            color(0.294118, 0.576471, 0.647059) // #4B93A5
        ];
    if (theme == PaletteTheme.FOREST)
        return [
            color(0.086275, 0.239216, 0.200000), // #163D33
            color(0.156863, 0.345098, 0.278431), // #285847
            color(0.203922, 0.462745, 0.345098), // #347658
            color(0.313725, 0.572549, 0.419608), // #50926B
            color(0.439216, 0.694118, 0.541176), // #70B18A
            color(0.647059, 0.831373, 0.674510), // #A5D4AC
            color(0.254902, 0.427451, 0.262745), // #416D43
            color(0.392157, 0.572549, 0.301961), // #64924D
            color(0.513725, 0.662745, 0.352941), // #83A95A
            color(0.725490, 0.811765, 0.521569), // #B9CF85
            color(0.149020, 0.486275, 0.443137), // #267C71
            color(0.400000, 0.662745, 0.600000) // #66A999
        ];
    if (theme == PaletteTheme.SUNSET)
        return [
            color(0.490196, 0.200000, 0.278431), // #7D3347
            color(0.705882, 0.266667, 0.290196), // #B4444A
            color(0.819608, 0.364706, 0.286275), // #D15D49
            color(0.909804, 0.505882, 0.278431), // #E88147
            color(0.960784, 0.639216, 0.364706), // #F5A35D
            color(0.972549, 0.772549, 0.521569), // #F8C585
            color(0.619608, 0.305882, 0.454902), // #9E4E74
            color(0.784314, 0.423529, 0.427451), // #C86C6D
            color(0.862745, 0.517647, 0.376471), // #DC8460
            color(0.913725, 0.654902, 0.474510), // #E9A779
            color(0.670588, 0.352941, 0.266667), // #AB5A44
            color(0.949020, 0.729412, 0.631373) // #F2BAA1
        ];
    if (theme == PaletteTheme.LAVENDER)
        return [
            color(0.278431, 0.211765, 0.360784), // #47365C
            color(0.376471, 0.282353, 0.466667), // #604877
            color(0.466667, 0.352941, 0.603922), // #775A9A
            color(0.596078, 0.454902, 0.709804), // #9874B5
            color(0.701961, 0.611765, 0.823529), // #B39CD2
            color(0.815686, 0.760784, 0.894118), // #D0C2E4
            color(0.329412, 0.352941, 0.603922), // #545A9A
            color(0.462745, 0.482353, 0.756863), // #767BC1
            color(0.603922, 0.619608, 0.847059), // #9A9ED8
            color(0.752941, 0.717647, 0.917647), // #C0B7EA
            color(0.501961, 0.352941, 0.552941), // #805A8D
            color(0.654902, 0.494118, 0.662745) // #A77EA9
        ];
    if (theme == PaletteTheme.ROSE)
        return [
            color(0.419608, 0.203922, 0.301961), // #6B344D
            color(0.572549, 0.274510, 0.384314), // #924662
            color(0.717647, 0.345098, 0.474510), // #B75879
            color(0.831373, 0.458824, 0.568627), // #D47591
            color(0.913725, 0.607843, 0.694118), // #E99BB1
            color(0.952941, 0.764706, 0.819608), // #F3C3D1
            color(0.682353, 0.321569, 0.435294), // #AE526F
            color(0.772549, 0.419608, 0.533333), // #C56B88
            color(0.847059, 0.560784, 0.666667), // #D88FAA
            color(0.921569, 0.717647, 0.780392), // #EBB7C7
            color(0.545098, 0.301961, 0.392157), // #8B4D64
            color(0.792157, 0.478431, 0.576471) // #CA7A93
        ];
    if (theme == PaletteTheme.EARTH)
        return [
            color(0.286275, 0.290196, 0.270588), // #494A45
            color(0.403922, 0.356863, 0.298039), // #675B4C
            color(0.501961, 0.427451, 0.352941), // #806D5A
            color(0.603922, 0.509804, 0.419608), // #9A826B
            color(0.721569, 0.635294, 0.545098), // #B8A28B
            color(0.835294, 0.768627, 0.670588), // #D5C4AB
            color(0.337255, 0.411765, 0.364706), // #56695D
            color(0.454902, 0.517647, 0.454902), // #748474
            color(0.572549, 0.619608, 0.560784), // #929E8F
            color(0.701961, 0.717647, 0.658824), // #B3B7A8
            color(0.521569, 0.435294, 0.447059), // #856F72
            color(0.701961, 0.603922, 0.596078) // #B39A98
        ];
    if (theme == PaletteTheme.GRAPHITE)
        return [
            color(0.156863, 0.203922, 0.262745), // #283443
            color(0.235294, 0.313725, 0.396078), // #3C5065
            color(0.317647, 0.415686, 0.482353), // #516A7B
            color(0.435294, 0.529412, 0.572549), // #6F8792
            color(0.596078, 0.662745, 0.686275), // #98A9AF
            color(0.756863, 0.803922, 0.815686), // #C1CDD0
            color(0.200000, 0.274510, 0.325490), // #334653
            color(0.290196, 0.376471, 0.439216), // #4A6070
            color(0.396078, 0.486275, 0.521569), // #657C85
            color(0.521569, 0.600000, 0.631373), // #8599A1
            color(0.658824, 0.713725, 0.756863), // #A8B6C1
            color(0.349020, 0.403922, 0.450980) // #596773
        ];
    if (theme == PaletteTheme.PASTEL)
        return [
            color(0.662745, 0.772549, 0.909804), // #A9C5E8
            color(0.662745, 0.847059, 0.823529), // #A9D8D2
            color(0.729412, 0.835294, 0.662745), // #BAD5A9
            color(0.886275, 0.827451, 0.623529), // #E2D39F
            color(0.913725, 0.717647, 0.694118), // #E9B7B1
            color(0.788235, 0.709804, 0.866667), // #C9B5DD
            color(0.541176, 0.713725, 0.819608), // #8AB6D1
            color(0.721569, 0.862745, 0.788235), // #B8DCC9
            color(0.815686, 0.850980, 0.686275), // #D0D9AF
            color(0.941176, 0.803922, 0.698039), // #F0CDB2
            color(0.843137, 0.674510, 0.792157), // #D7ACCA
            color(0.678431, 0.721569, 0.858824) // #ADB8DB
        ];
    if (theme == PaletteTheme.ICE)
        return [
            color(0.164706, 0.317647, 0.427451), // #2A516D
            color(0.227451, 0.443137, 0.541176), // #3A718A
            color(0.337255, 0.588235, 0.678431), // #5696AD
            color(0.498039, 0.717647, 0.780392), // #7FB7C7
            color(0.698039, 0.850980, 0.878431), // #B2D9E0
            color(0.823529, 0.921569, 0.925490), // #D2EBEC
            color(0.207843, 0.490196, 0.607843), // #357D9B
            color(0.364706, 0.670588, 0.764706), // #5DABC3
            color(0.541176, 0.803922, 0.858824), // #8ACDDB
            color(0.690196, 0.843137, 0.909804), // #B0D7E8
            color(0.301961, 0.458824, 0.596078), // #4D7598
            color(0.564706, 0.725490, 0.831373) // #90B9D4
        ];
    if (theme == PaletteTheme.COPPER)
        return [
            color(0.349020, 0.258824, 0.215686), // #594237
            color(0.466667, 0.317647, 0.254902), // #775141
            color(0.600000, 0.388235, 0.270588), // #996345
            color(0.733333, 0.478431, 0.305882), // #BB7A4E
            color(0.831373, 0.603922, 0.435294), // #D49A6F
            color(0.890196, 0.725490, 0.580392), // #E3B994
            color(0.403922, 0.301961, 0.239216), // #674D3D
            color(0.619608, 0.423529, 0.329412), // #9E6C54
            color(0.749020, 0.560784, 0.419608), // #BF8F6B
            color(0.823529, 0.674510, 0.525490), // #D2AC86
            color(0.549020, 0.403922, 0.294118), // #8C674B
            color(0.713725, 0.521569, 0.380392) // #B68561
        ];
    if (theme == PaletteTheme.NEON)
        return [
            color(0.086275, 0.274510, 0.341176), // #164657
            color(0.082353, 0.419608, 0.533333), // #156B88
            color(0.054902, 0.607843, 0.666667), // #0E9BAA
            color(0.156863, 0.768627, 0.717647), // #28C4B7
            color(0.392157, 0.894118, 0.772549), // #64E4C5
            color(0.568627, 0.917647, 0.843137), // #91EAD7
            color(0.356863, 0.262745, 0.627451), // #5B43A0
            color(0.470588, 0.349020, 0.760784), // #7859C2
            color(0.623529, 0.462745, 0.819608), // #9F76D1
            color(0.768627, 0.611765, 0.878431), // #C49CE0
            color(0.223529, 0.470588, 0.745098), // #3978BE
            color(0.364706, 0.658824, 0.909804) // #5DA8E8
        ];
    if (theme == PaletteTheme.CONTRAST)
        return [
            color(0.164706, 0.388235, 0.556863), // #2A638E
            color(0.800000, 0.431373, 0.274510), // #CC6E46
            color(0.309804, 0.529412, 0.380392), // #4F8761
            color(0.639216, 0.419608, 0.623529), // #A36B9F
            color(0.815686, 0.639216, 0.290196), // #D0A34A
            color(0.301961, 0.568627, 0.627451), // #4D91A0
            color(0.654902, 0.372549, 0.380392), // #A75F61
            color(0.454902, 0.549020, 0.309804), // #748C4F
            color(0.478431, 0.466667, 0.670588), // #7A77AB
            color(0.729412, 0.517647, 0.376471), // #BA8460
            color(0.298039, 0.447059, 0.521569), // #4C7285
            color(0.717647, 0.666667, 0.447059) // #B7AA72
        ];
    if (theme == PaletteTheme.MONOCHROME)
        return [
            color(0.058824, 0.066667, 0.078431), // #0F1114
            color(0.113725, 0.129412, 0.149020), // #1D2126
            color(0.203922, 0.227451, 0.250980), // #343A40
            color(0.929412, 0.925490, 0.909804), // #EDECE8
            color(1.000000, 1.000000, 1.000000), // #FFFFFF
            color(0.780392, 0.803922, 0.815686), // #C7CDD0
            color(0.082353, 0.101961, 0.125490), // #151A20
            color(0.968627, 0.968627, 0.956863), // #F7F7F4
            color(0.392157, 0.439216, 0.478431), // #64707A
            color(0.670588, 0.694118, 0.705882), // #ABB1B4
            color(0.145098, 0.172549, 0.196078), // #252C32
            color(0.850980, 0.866667, 0.878431) // #D9DDE0
        ];
    if (theme == PaletteTheme.IVORY_GOLD)
        return [
            color(0.968627, 0.960784, 0.933333), // #F7F5EE
            color(0.909804, 0.866667, 0.768627), // #E8DDC4
            color(0.780392, 0.662745, 0.411765), // #C7A969
            color(0.647059, 0.498039, 0.207843), // #A57F35
            color(0.941176, 0.909804, 0.839216), // #F0E8D6
            color(0.145098, 0.152941, 0.168627), // #25272B
            color(0.980392, 0.972549, 0.941176), // #FAF8F0
            color(0.831373, 0.737255, 0.525490), // #D4BC86
            color(0.600000, 0.509804, 0.372549), // #99825F
            color(0.925490, 0.917647, 0.898039), // #ECEAE5
            color(0.364706, 0.345098, 0.317647), // #5D5851
            color(0.737255, 0.631373, 0.427451) // #BCA16D
        ];
    if (theme == PaletteTheme.BLACK_GOLD)
        return [
            color(0.058824, 0.070588, 0.086275), // #0F1216
            color(0.113725, 0.133333, 0.156863), // #1D2228
            color(0.709804, 0.552941, 0.282353), // #B58D48
            color(0.819608, 0.682353, 0.411765), // #D1AE69
            color(0.160784, 0.184314, 0.207843), // #292F35
            color(0.619608, 0.474510, 0.215686), // #9E7937
            color(0.090196, 0.098039, 0.113725), // #17191D
            color(0.878431, 0.776471, 0.545098), // #E0C68B
            color(0.250980, 0.274510, 0.290196), // #40464A
            color(0.749020, 0.635294, 0.423529), // #BFA26C
            color(0.141176, 0.156863, 0.184314), // #24282F
            color(0.788235, 0.690196, 0.431373) // #C9B06E
        ];
    if (theme == PaletteTheme.STEALTH_BLACK)
        return [
            color(0.062745, 0.066667, 0.078431), // #101114
            color(0.101961, 0.113725, 0.129412), // #1A1D21
            color(0.145098, 0.164706, 0.188235), // #252A30
            color(0.188235, 0.215686, 0.250980), // #303740
            color(0.043137, 0.047059, 0.054902), // #0B0C0E
            color(0.270588, 0.298039, 0.329412), // #454C54
            color(0.078431, 0.090196, 0.101961), // #14171A
            color(0.207843, 0.239216, 0.270588), // #353D45
            color(0.125490, 0.145098, 0.164706), // #20252A
            color(0.317647, 0.345098, 0.380392), // #515861
            color(0.090196, 0.109804, 0.125490), // #171C20
            color(0.243137, 0.270588, 0.305882) // #3E454E
        ];
    if (theme == PaletteTheme.BLACK_RED)
        return [
            color(0.066667, 0.078431, 0.090196), // #111417
            color(0.137255, 0.152941, 0.172549), // #23272C
            color(0.670588, 0.125490, 0.168627), // #AB202B
            color(0.823529, 0.223529, 0.250980), // #D23940
            color(0.188235, 0.219608, 0.254902), // #303841
            color(0.945098, 0.909804, 0.886275), // #F1E8E2
            color(0.082353, 0.105882, 0.129412), // #151B21
            color(0.517647, 0.113725, 0.156863), // #841D28
            color(0.607843, 0.670588, 0.701961), // #9BABB3
            color(0.717647, 0.176471, 0.215686), // #B72D37
            color(0.258824, 0.278431, 0.305882), // #42474E
            color(0.419608, 0.109804, 0.145098) // #6B1C25
        ];
    if (theme == PaletteTheme.WHITE_TEAL)
        return [
            color(0.956863, 0.960784, 0.945098), // #F4F5F1
            color(0.870588, 0.909804, 0.901961), // #DEE8E6
            color(0.101961, 0.560784, 0.596078), // #1A8F98
            color(0.149020, 0.698039, 0.709804), // #26B2B5
            color(0.207843, 0.231373, 0.258824), // #353B42
            color(0.647059, 0.803922, 0.815686), // #A5CDD0
            color(1.000000, 1.000000, 1.000000), // #FFFFFF
            color(0.031373, 0.462745, 0.498039), // #08767F
            color(0.717647, 0.768627, 0.776471), // #B7C4C6
            color(0.415686, 0.611765, 0.627451), // #6A9CA0
            color(0.898039, 0.921569, 0.909804), // #E5EBE8
            color(0.231373, 0.466667, 0.501961) // #3B7780
        ];
    if (theme == PaletteTheme.TIDAL_TEAL)
        return [
            color(0.078431, 0.129412, 0.152941), // #142127
            color(0.121569, 0.192157, 0.219608), // #1F3138
            color(0.047059, 0.490196, 0.521569), // #0C7D85
            color(0.082353, 0.662745, 0.670588), // #15A9AB
            color(0.564706, 0.749020, 0.760784), // #90BFC2
            color(0.839216, 0.898039, 0.882353), // #D6E5E1
            color(0.070588, 0.101961, 0.125490), // #121A20
            color(0.219608, 0.721569, 0.713725), // #38B8B6
            color(0.282353, 0.407843, 0.443137), // #486871
            color(0.145098, 0.498039, 0.533333), // #257F88
            color(0.219608, 0.262745, 0.294118), // #38434B
            color(0.650980, 0.807843, 0.815686) // #A6CED0
        ];
    if (theme == PaletteTheme.WHITE_RED_GOLD)
        return [
            color(0.968627, 0.960784, 0.941176), // #F7F5F0
            color(0.843137, 0.784314, 0.627451), // #D7C8A0
            color(0.764706, 0.635294, 0.341176), // #C3A257
            color(0.537255, 0.180392, 0.200000), // #892E33
            color(0.133333, 0.145098, 0.172549), // #22252C
            color(0.909804, 0.850980, 0.717647), // #E8D9B7
            color(1.000000, 1.000000, 1.000000), // #FFFFFF
            color(0.725490, 0.258824, 0.282353), // #B94248
            color(0.945098, 0.909804, 0.827451), // #F1E8D3
            color(0.674510, 0.533333, 0.290196), // #AC884A
            color(0.427451, 0.188235, 0.203922), // #6D3034
            color(0.807843, 0.772549, 0.709804) // #CEC5B5
        ];
    if (theme == PaletteTheme.VIOLET_CIRCUIT)
        return [
            color(0.078431, 0.082353, 0.109804), // #14151C
            color(0.172549, 0.164706, 0.219608), // #2C2A38
            color(0.341176, 0.258824, 0.556863), // #57428E
            color(0.513725, 0.400000, 0.721569), // #8366B8
            color(0.141176, 0.164706, 0.176471), // #242A2D
            color(0.654902, 0.819608, 0.317647), // #A7D151
            color(0.098039, 0.113725, 0.137255), // #191D23
            color(0.431373, 0.333333, 0.647059), // #6E55A5
            color(0.701961, 0.643137, 0.811765), // #B3A4CF
            color(0.270588, 0.364706, 0.188235), // #455D30
            color(0.211765, 0.231373, 0.266667), // #363B44
            color(0.552941, 0.745098, 0.247059) // #8DBE3F
        ];
    if (theme == PaletteTheme.BLUE_ALUMINUM)
        return [
            color(0.827451, 0.854902, 0.874510), // #D3DADF
            color(0.086275, 0.290196, 0.529412), // #164A87
            color(0.145098, 0.400000, 0.639216), // #2566A3
            color(0.105882, 0.156863, 0.207843), // #1B2835
            color(0.945098, 0.952941, 0.945098), // #F1F3F1
            color(0.435294, 0.568627, 0.662745), // #6F91A9
            color(0.666667, 0.705882, 0.745098), // #AAB4BE
            color(0.054902, 0.211765, 0.415686), // #0E366A
            color(0.768627, 0.803922, 0.827451), // #C4CDD3
            color(0.262745, 0.490196, 0.666667), // #437DAA
            color(0.219608, 0.243137, 0.270588), // #383E45
            color(0.898039, 0.913725, 0.921569) // #E5E9EB
        ];
    if (theme == PaletteTheme.RAW_ALUMINUM)
        return [
            color(0.811765, 0.827451, 0.839216), // #CFD3D6
            color(0.666667, 0.698039, 0.729412), // #AAB2BA
            color(0.470588, 0.498039, 0.521569), // #787F85
            color(0.886275, 0.901961, 0.905882), // #E2E6E7
            color(0.105882, 0.125490, 0.145098), // #1B2025
            color(0.725490, 0.756863, 0.784314), // #B9C1C8
            color(0.588235, 0.623529, 0.658824), // #969FA8
            color(0.945098, 0.945098, 0.933333), // #F1F1EE
            color(0.396078, 0.427451, 0.454902), // #656D74
            color(0.847059, 0.862745, 0.874510), // #D8DCDF
            color(0.235294, 0.266667, 0.298039), // #3C444C
            color(0.733333, 0.760784, 0.776471) // #BBC2C6
        ];
    if (theme == PaletteTheme.WHITE_CARBON)
        return [
            color(0.964706, 0.968627, 0.964706), // #F6F7F6
            color(0.070588, 0.098039, 0.117647), // #12191E
            color(0.243137, 0.290196, 0.313725), // #3E4A50
            color(1.000000, 1.000000, 1.000000), // #FFFFFF
            color(0.654902, 0.698039, 0.721569), // #A7B2B8
            color(0.145098, 0.176471, 0.200000), // #252D33
            color(0.882353, 0.898039, 0.898039), // #E1E5E5
            color(0.082353, 0.113725, 0.141176), // #151D24
            color(0.419608, 0.474510, 0.501961), // #6B7980
            color(0.768627, 0.803922, 0.815686), // #C4CDD0
            color(0.203922, 0.235294, 0.262745), // #343C43
            color(0.929412, 0.941176, 0.937255) // #EDF0EF
        ];
    if (theme == PaletteTheme.ORANGE_GRAPHITE)
        return [
            color(0.141176, 0.152941, 0.164706), // #24272A
            color(0.937255, 0.443137, 0.117647), // #EF711E
            color(0.909804, 0.349020, 0.086275), // #E85916
            color(0.270588, 0.286275, 0.301961), // #45494D
            color(0.776471, 0.780392, 0.772549), // #C6C7C5
            color(0.968627, 0.635294, 0.364706), // #F7A25D
            color(0.188235, 0.211765, 0.231373), // #30363B
            color(0.619608, 0.278431, 0.098039), // #9E4719
            color(0.678431, 0.698039, 0.705882), // #ADB2B4
            color(0.945098, 0.552941, 0.219608), // #F18D38
            color(0.364706, 0.384314, 0.400000), // #5D6266
            color(0.850980, 0.862745, 0.866667) // #D9DCDD
        ];
    if (theme == PaletteTheme.SAFETY_YELLOW)
        return [
            color(0.133333, 0.145098, 0.160784), // #222529
            color(0.964706, 0.788235, 0.074510), // #F6C913
            color(0.898039, 0.701961, 0.098039), // #E5B319
            color(0.231373, 0.254902, 0.282353), // #3B4148
            color(0.929412, 0.921569, 0.894118), // #EDEBE4
            color(1.000000, 0.886275, 0.411765), // #FFE269
            color(0.082353, 0.098039, 0.121569), // #15191F
            color(0.788235, 0.600000, 0.090196), // #C99917
            color(0.552941, 0.572549, 0.600000), // #8D9299
            color(0.949020, 0.803922, 0.215686), // #F2CD37
            color(0.356863, 0.384314, 0.415686), // #5B626A
            color(0.850980, 0.854902, 0.866667) // #D9DADD
        ];
    if (theme == PaletteTheme.COBOT_WHITE)
        return [
            color(0.972549, 0.980392, 0.980392), // #F8FAFA
            color(0.847059, 0.866667, 0.882353), // #D8DDE1
            color(0.658824, 0.694118, 0.721569), // #A8B1B8
            color(0.905882, 0.917647, 0.925490), // #E7EAEC
            color(0.423529, 0.470588, 0.505882), // #6C7881
            color(1.000000, 1.000000, 1.000000), // #FFFFFF
            color(0.815686, 0.839216, 0.854902), // #D0D6DA
            color(0.756863, 0.784314, 0.807843), // #C1C8CE
            color(0.584314, 0.623529, 0.650980), // #959FA6
            color(0.937255, 0.949020, 0.945098), // #EFF2F1
            color(0.301961, 0.349020, 0.384314), // #4D5962
            color(0.729412, 0.764706, 0.784314) // #BAC3C8
        ];
    if (theme == PaletteTheme.COBOT_SILVER_BLUE)
        return [
            color(0.850980, 0.874510, 0.882353), // #D9DFE1
            color(0.709804, 0.745098, 0.768627), // #B5BEC4
            color(0.145098, 0.200000, 0.247059), // #25333F
            color(0.133333, 0.423529, 0.670588), // #226CAB
            color(0.219608, 0.266667, 0.305882), // #38444E
            color(0.949020, 0.956863, 0.960784), // #F2F4F5
            color(0.600000, 0.658824, 0.701961), // #99A8B3
            color(0.066667, 0.321569, 0.501961), // #115280
            color(0.305882, 0.498039, 0.639216), // #4E7FA3
            color(0.870588, 0.894118, 0.905882), // #DEE4E7
            color(0.337255, 0.388235, 0.435294), // #56636F
            color(0.776471, 0.823529, 0.854902) // #C6D2DA
        ];
    if (theme == PaletteTheme.SPOT_YELLOW)
        return [
            color(0.082353, 0.098039, 0.117647), // #15191E
            color(0.956863, 0.768627, 0.149020), // #F4C426
            color(0.874510, 0.682353, 0.113725), // #DFAE1D
            color(0.188235, 0.219608, 0.250980), // #303840
            color(0.980392, 0.850980, 0.294118), // #FAD94B
            color(0.662745, 0.686275, 0.694118), // #A9AFB1
            color(0.113725, 0.141176, 0.164706), // #1D242A
            color(0.827451, 0.650980, 0.149020), // #D3A626
            color(0.349020, 0.368627, 0.376471), // #595E60
            color(1.000000, 0.882353, 0.419608), // #FFE16B
            color(0.278431, 0.309804, 0.341176), // #474F57
            color(0.721569, 0.745098, 0.741176) // #B8BEBD
        ];
    if (theme == PaletteTheme.TITANIUM_CARBON)
        return [
            color(0.133333, 0.152941, 0.180392), // #22272E
            color(0.396078, 0.443137, 0.478431), // #65717A
            color(0.227451, 0.262745, 0.298039), // #3A434C
            color(0.643137, 0.686275, 0.705882), // #A4AFB4
            color(0.074510, 0.094118, 0.125490), // #131820
            color(0.470588, 0.525490, 0.556863), // #78868E
            color(0.317647, 0.360784, 0.400000), // #515C66
            color(0.796078, 0.823529, 0.835294), // #CBD2D5
            color(0.188235, 0.227451, 0.270588), // #303A45
            color(0.611765, 0.647059, 0.666667), // #9CA5AA
            color(0.117647, 0.149020, 0.188235), // #1E2630
            color(0.717647, 0.752941, 0.768627) // #B7C0C4
        ];
    // Explicit fallback to avoid undefined color on future enum changes.
    return [color(0.3, 0.5, 0.7)];
}

function variationOffset(v is PaletteVariation) returns number
{
    if (v == PaletteVariation.V2) return 1;
    if (v == PaletteVariation.V3) return 3;
    if (v == PaletteVariation.V4) return 5;
    if (v == PaletteVariation.V5) return 7;
    if (v == PaletteVariation.V6) return 9;
    if (v == PaletteVariation.V7) return 10;
    if (v == PaletteVariation.V8) return 11;
    return 0;
}

function paletteIndex(partIndex is number, paletteCount is number,
                      shift is number, arrangement is PaletteArrangement) returns number
{
    // Deterministic so feature regeneration yields stable, reproducible results.
    if (arrangement == PaletteArrangement.ALTERNATE)
        return (partIndex * 5 + shift) % paletteCount;
    if (arrangement == PaletteArrangement.SPREAD)
        return (partIndex * 7 + shift) % paletteCount;
    if (arrangement == PaletteArrangement.REVERSE)
        return paletteCount - 1 - ((partIndex + shift) % paletteCount);
    return (partIndex + shift) % paletteCount;
}

annotation {
    "Feature Type Name" : "Palette Studio",
    "Feature Type Description" : "Apply coordinated color themes to solid parts. Source part appearances are inherited by assemblies."
}
export const paletteStudio = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Palette" }
        definition.palette is PaletteTheme;

        annotation { "Name" : "Color scope" }
        definition.scope is PaletteScope;

        if (definition.scope == PaletteScope.SELECTED)
        {
            annotation { "Name" : "Parts to color", "Filter" : EntityType.BODY && BodyType.SOLID }
            definition.parts is Query;
        }

        annotation { "Name" : "Arrangement" }
        definition.arrangement is PaletteArrangement;

        annotation { "Name" : "Variation" }
        definition.variation is PaletteVariation;
    }
    {
        var target = qBodyType(qEverything(EntityType.BODY), BodyType.SOLID);
        if (definition.scope == PaletteScope.SELECTED)
            target = qBodyType(definition.parts, BodyType.SOLID);

        const bodies = evaluateQuery(context, target);
        const colors = paletteColors(definition.palette);
        const shift = variationOffset(definition.variation);

        for (var i = 0; i < size(bodies); i += 1)
        {
            const index = paletteIndex(i, size(colors), shift, definition.arrangement);
            setProperty(context, {
                "entities" : bodies[i],
                "propertyType" : PropertyType.APPEARANCE,
                "value" : colors[index]
            });
        }
    });