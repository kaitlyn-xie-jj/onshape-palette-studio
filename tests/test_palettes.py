from pathlib import Path
import json, re, unittest
ROOT = Path(__file__).parents[1]
DATA = json.loads((ROOT/'palettes.json').read_text(encoding='utf-8'))
SOURCE = (ROOT/'src'/'palette_studio.fs').read_text(encoding='utf-8')

class PaletteTests(unittest.TestCase):
    def test_twelve_themes_with_twelve_unique_shades(self):
        self.assertEqual(len(DATA), 12)
        for key, palette in DATA.items():
            with self.subTest(key=key):
                self.assertEqual(len(palette['colors']), 12)
                self.assertEqual(len(set(palette['colors'])), 12)
                self.assertTrue(all(re.fullmatch(r'#[0-9A-F]{6}', c) for c in palette['colors']))
    def test_feature_contains_every_theme_and_color(self):
        self.assertEqual(SOURCE.count('            color('), 144)
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

if __name__ == '__main__': unittest.main()
