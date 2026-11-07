from pathlib import Path
import json, re, unittest
ROOT = Path(__file__).parents[1]
DATA = json.loads((ROOT/'palettes.json').read_text(encoding='utf-8'))
SOURCE = (ROOT/'src'/'palette_studio.fs').read_text(encoding='utf-8')

class PaletteTests(unittest.TestCase):
    def test_thirty_themes_with_twelve_unique_shades(self):
        self.assertEqual(len(DATA), 30)
        for key, palette in DATA.items():
            with self.subTest(key=key):
                self.assertEqual(len(palette['colors']), 12)
                self.assertEqual(len(set(palette['colors'])), 12)
                self.assertTrue(all(re.fullmatch(r'#[0-9A-F]{6}', c) for c in palette['colors']))
    def test_feature_contains_every_theme_and_color(self):
        self.assertEqual(SOURCE.count('            color('), 360)
        for theme in DATA:
            self.assertIn('PaletteTheme.'+theme, SOURCE)
        for p in DATA.values():
            for color in p['colors']:
                self.assertIn('// '+color, SOURCE)
    def test_every_variation_is_permutation_of_colors(self):
        variants = [0,1,3,5,7,9,10,11]
        for v in variants:
            for arrangement in ('GRADIENT','ALTERNATE','SPREAD','REVERSE'):
                ids=[]
                for i in range(12):
                    if arrangement == 'ALTERNATE': j=(i*5+v)%12
                    elif arrangement == 'SPREAD': j=(i*7+v)%12
                    elif arrangement == 'REVERSE': j=11-((i+v)%12)
                    else: j=(i+v)%12
                    ids.append(j)
                self.assertEqual(set(ids),set(range(12)))
    def test_preview_embeds_data_offline(self):
        html=(ROOT/'preview'/'palette-preview.html').read_text(encoding='utf-8')
        self.assertNotIn('__PALETTE_DATA__', html)
        self.assertIn('application/json', html)
        self.assertIn('generate', html.lower()+'generate')

    def test_category_counts_and_reference_metadata(self):
        self.assertEqual({name: sum(p.get('category') == name for p in DATA.values())
                          for name in ('Original', 'FRC', 'Industrial')},
                         {'Original': 12, 'FRC': 12, 'Industrial': 6})
        for name, p in DATA.items():
            with self.subTest(name=name):
                self.assertTrue(p.get('description'))
                if p['category'] != 'Original':
                    self.assertTrue(p.get('source', '').startswith('https://'))

    def test_new_mechanical_color_families_exist(self):
        for name in ('MONOCHROME', 'IVORY_GOLD', 'BLACK_GOLD',
                     'STEALTH_BLACK', 'WHITE_TEAL', 'TIDAL_TEAL',
                     'ORANGE_GRAPHITE', 'SAFETY_YELLOW', 'COBOT_WHITE',
                     'SPOT_YELLOW', 'TITANIUM_CARBON'):
            self.assertIn(name, DATA)

    def test_original_palette_endpoints_preserved(self):
        endpoints = {
            'OCEAN': ('#123A5B', '#4B93A5'),
            'FOREST': ('#163D33', '#66A999'),
            'SUNSET': ('#7D3347', '#F2BAA1'),
            'LAVENDER': ('#47365C', '#A77EA9'),
            'ROSE': ('#6B344D', '#CA7A93'),
            'EARTH': ('#494A45', '#B39A98'),
            'GRAPHITE': ('#283443', '#596773'),
            'PASTEL': ('#A9C5E8', '#ADB8DB'),
            'ICE': ('#2A516D', '#90B9D4'),
            'COPPER': ('#594237', '#B68561'),
            'NEON': ('#164657', '#5DA8E8'),
            'CONTRAST': ('#2A638E', '#B7AA72'),
        }
        for key, (first, last) in endpoints.items():
            self.assertEqual((DATA[key]['colors'][0], DATA[key]['colors'][-1]), (first, last))

    def test_preview_has_filters_and_embedded_palettes(self):
        preview = (ROOT/'preview'/'palette-preview.html').read_text(encoding='utf-8')
        self.assertIn('data-filter', preview)
        self.assertIn('source-link', preview)
        for key in DATA:
            self.assertIn('"'+key+'":', preview)

if __name__ == '__main__': unittest.main()
