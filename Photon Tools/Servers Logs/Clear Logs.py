import os
from os import walk

directory_path = os.path.dirname(os.path.realpath(__file__))

for (directory, _, files) in os.walk(directory_path):
     for file in files:
        path = os.path.join(directory, file)
        if path.endswith('.log'):
          log = open(path, 'r+')
          log.truncate()
          log.close()
