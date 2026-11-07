// Onshape Palette Studio v0.1 — native FeatureScript custom feature.
// Apply to source Part Studios; appearances flow through to assemblies.
// FeatureScript 2543 is a conservative, proven-compatible language version.
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
    CONTRAST
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
    annotation { "Name" : "Soft gradient" }
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