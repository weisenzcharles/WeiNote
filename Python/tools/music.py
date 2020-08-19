# --conding=utf8  --
import os
import eyed3
import mutagen
from mutagen.mp3 import MP3
from mutagen.flac import FLAC
from mutagen.id3 import ID3, TIT2
# audiofile = eyed3.load("song.flac")
# audiofile.tag.artist = u"Nobunny"
# audiofile.tag.album = u"Love Visions"
# audiofile.tag.title = u"I Am a Girlfriend"
# audiofile.tag.track_num = 4

# audiofile.tag.save()


prefix = r".mp3"

for file in os.listdir(r"C:\Users\Charles Zhang\Desktop\Music-Downloader-master"):
    # print(os.path.splitext(file))
    fileext = os.path.splitext(file)[1]

    if(fileext == '.mp3'):
        audio = ID3(file)

        print(mutagen.File(file).tags)
        audio["title"] = mutagen.id3.TextFrame(
            encoding=3, text=[u"hahahaah"])
        audio.add(TIT2(encoding=3, text=u"An example"))
        audio.save()
        # mp3audio = MP3(file)
        # mp3audio["title"] = mutagen.id3.TextFrame(
        #     encoding=3, text=[u"hahahaah"])
        # mp3audio.pprint()
        # mp3audio.save()
        print(file)

    elif (fileext == '.flac'):
        audio = FLAC(file)
        audio["title"] = u"An example"
        audio.pprint()
        audio.save()
        print(file)
