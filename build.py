"""Regenerate FeatureScript palettes, standalone HTML preview, and SVG gallery from palettes.json.

Run: python build.py
The core feature logic outside PaletteTheme/paletteColors remains intact.
"""
from pathlib import Path
import json
import re
import html

ROOT = Path(__file__).resolve().parent
DATA = json.loads((ROOT / "palettes.json").read_text(encoding="utf-8"))
FEATURE = ROOT / "src" / "palette_studio.fs"

def rgb(hex_code):
    if not re.fullmatch(r"#[0-9A-Fa-f]{6}", hex_code):
        raise ValueError(f"Invalid hex colour: {hex_code}")
    return [int(hex_code[i:i + 2], 16) / 255 for i in (1, 3, 5)]

def theme_enum():
    lines = ["export enum PaletteTheme", "{"]
    entries = list(DATA.items())
    for index, (key, entry) in enumerate(entries):
        if not re.fullmatch(r"[A-Z][A-Z0-9_]*", key):
            raise ValueError(f"Invalid FeatureScript enum identifier: {key}")
        name = entry["name"].replace("\\", "\\\\").replace('"', '\\"')
        lines.append(f'    annotation {{ "Name" : "{name}" }}')
        lines.append(f'    {key}' + ("," if index < len(entries) - 1 else ""))
    lines.append("}")
    return "\n".join(lines)

def colors_function():
    lines = ["function paletteColors(theme is PaletteTheme) returns array", "{"]
    for key, entry in DATA.items():
        shades = entry["colors"]
        if not shades:
            raise ValueError(f"Palette {key} has no colours")
        lines.extend([f"    if (theme == PaletteTheme.{key})", "        return ["])
        for index, hex_code in enumerate(shades):
            numbers = ", ".join(f"{v:.6f}" for v in rgb(hex_code))
            comma = "," if index < len(shades) - 1 else ""
            lines.append(f"            color({numbers}){comma} // {hex_code.upper()}")
        lines.append("        ];")
    lines.extend([
        "    // Explicit fallback to avoid undefined color on future enum changes.",
        "    return [color(0.3, 0.5, 0.7)];",
        "}"
    ])
    return "\n".join(lines)


def write_preview():
    template = (ROOT / "preview_template.html").read_text(encoding="utf-8")
    payload = json.dumps(DATA, ensure_ascii=False, separators=(",", ":")).replace("</", "<\\/")
    (ROOT / "preview" / "palette-preview.html").write_text(
        template.replace("__PALETTE_DATA__", payload), encoding="utf-8")

def write_gallery():
    categories = ("FRC", "Industrial", "Original")
    ordered = sorted(DATA.items(), key=lambda kv: (
        categories.index(kv[1].get("category", "Original")), kv[0]))
    col_width, row_height = 630, 104
    w, h = 1320, 145 + ((len(ordered) + 1) // 2) * row_height
    svg = [
        f'<svg xmlns="http://www.w3.org/2000/svg" width="{w}" height="{h}" viewBox="0 0 {w} {h}">',
        f'<rect width="{w}" height="{h}" fill="#0b111b"/>',
        '<text x="32" y="52" font-size="30" font-family="Arial, sans-serif" font-weight="bold" fill="#f2f5fa">Onshape Palette Studio · curated robot palettes</text>',
        f'<text x="32" y="82" font-size="17" font-family="Arial, sans-serif" fill="#aebed0">{len(DATA)} palettes · {len(DATA)*12} colors · FRC / industrial / original</text>'
    ]
    for i, (key, palette) in enumerate(ordered):
        x, y = 30 + (i % 2) * col_width, 107 + (i // 2) * row_height
        svg += [
            f'<rect x="{x}" y="{y}" width="610" height="91" rx="12" fill="#142134" stroke="#304154"/>',
            f'<text x="{x+13}" y="{y+25}" font-size="16" fill="#ecf3fc" font-family="Arial, sans-serif">{html.escape(palette["name"])}</text>',
            f'<text x="{x+500}" y="{y+25}" font-size="11" fill="#9fbbd9" font-family="Arial, sans-serif">{palette.get("category", "Original")}</text>'
        ]
        for j, color in enumerate(palette["colors"]):
            svg.append(f'<rect x="{x+13+j*48}" y="{y+38}" width="46" height="38" rx="3" fill="{color}"/>')
    svg.append("</svg>")
    (ROOT / "preview" / "palette-gallery.svg").write_text("\n".join(svg) + "\n", encoding="utf-8")

def main():
    source = FEATURE.read_text(encoding="utf-8")
    enum_begin = source.index("export enum PaletteTheme")
    enum_end = source.index("export enum PaletteScope", enum_begin)
    source = source[:enum_begin] + theme_enum() + "\n\n" + source[enum_end:]
    fn_begin = source.index("function paletteColors(")
    fn_end = source.index("function variationOffset(", fn_begin)
    source = source[:fn_begin] + colors_function() + "\n\n" + source[fn_end:]
    FEATURE.write_text(source, encoding="utf-8")
    write_preview()
    write_gallery()
    print(f"Generated {len(DATA)} themes and {sum(len(p['colors']) for p in DATA.values())} colours")

if __name__ == "__main__":
    main()
