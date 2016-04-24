set PATH=%PATH%;D:\Programs\ffmpeg\ffmpeg-20160422-git-268b5ae-win32-static\bin

rem ffmpeg -f image2 -i image%%3d.jpg video.avi
rem ffmpeg -i video.avi -pix_fmt rgb24 -loop_output 0 out.gif
rem ffmpeg -f image2 -i image%%3d.jpg out.gif

ffmpeg -i out.gif -filter:v "crop=200:300:610:417" out1.gif