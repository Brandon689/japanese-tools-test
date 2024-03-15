from pathlib import Path
#import cv2
import numpy as np
#from PIL import Image
from loguru import logger
#from scipy.signal.windows import gaussian
from tqdm import tqdm
#import json
#import requests
import argparse

from comic_text_detector.inference import TextDetector
from manga_ocr import MangaOcr
from manga_page_ocr import MangaPageOcr

from sautils import dump_json, load_json #, imread
from cache import cache



class OverlayGenerator:
    def __init__(self,
                 pretrained_model_name_or_path='kha-white/manga-ocr-base',
                 force_cpu=False,
                 **kwargs):
        self.pretrained_model_name_or_path = pretrained_model_name_or_path
        self.force_cpu = force_cpu
        self.kwargs = kwargs
        self.mpocr = None

    def init_models(self):
        if self.mpocr is None:
            self.mpocr = MangaPageOcr(self.pretrained_model_name_or_path, self.force_cpu, **self.kwargs)

    def process_dir(self, path, as_one_file=True, is_demo=False):
        path = Path(path).expanduser().absolute()
        assert path.is_dir(), f'{path} must be a directory'
        if path.stem == '_ocr':
            logger.info(f'Skipping OCR directory: {path}')
            return
        out_dir = path.parent

        results_dir = out_dir / '_ocr' / path.name
        results_dir.mkdir(parents=True, exist_ok=True)

        img_paths = [p for p in sorted(path.glob('**/*')) if p.is_file() and p.suffix.lower() in ('.jpg', '.jpeg', '.png')]

        page_htmls = []

        for img_path in tqdm(img_paths, desc='Processing pages...'):
            json_path = (results_dir / img_path.relative_to(path)).with_suffix('.json')

            print(json_path)

            if json_path.is_file():
                result = load_json(json_path)
                print('load result')
                print(json_path)
            else:
                self.init_models()
                result = self.mpocr(img_path)
                json_path.parent.mkdir(parents=True, exist_ok=True)
                dump_json(result, json_path)

cache = cache()

ovg = OverlayGenerator(pretrained_model_name_or_path='kha-white/manga-ocr-base', force_cpu=False)

parser = argparse.ArgumentParser()
parser.add_argument("folder")
args = parser.parse_args()

ovg.process_dir("C:\\python\\watch", as_one_file=True)
