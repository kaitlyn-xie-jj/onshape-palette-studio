"""Regenerate the FeatureScript theme definitions from palettes.json.

Run: python build.py
This updates only PaletteTheme and paletteColors; feature logic stays intact.
"""
from pathlib import Path
import json
import re

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

def main():
    source = FEATURE.read_text(encoding="utf-8")
    enum_begin = source.index("export enum PaletteTheme")
    enum_end = source.index("export enum PaletteScope", enum_begin)
    source = source[:enum_begin] + theme_enum() + "\n\n" + source[enum_end:]
    fn_begin = source.index("function paletteColors(")
    fn_end = source.index("function variationOffset(", fn_begin)
    source = source[:fn_begin] + colors_function() + "\n\n" + source[fn_end:]
    FEATURE.write_text(source, encoding="utf-8")
    print(f"Generated {len(DATA)} themes and {sum(len(p['colors']) for p in DATA.values())} colours")

if __name__ == "__main__":
    main()
