import os
import shutil
files = [f for f in os.listdir('.') if os.path.isfile(f) and f.endswith("jpg")]
files = sorted(files)

for i in xrange(len(files)):
    shutil.copy(files[i], "video/image%03d.jpg"%i)