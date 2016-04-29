import os, numpy, PIL
from PIL import Image

def paging(BASE_INDEX):
    # Access all PNG files in directory
    allfiles=os.listdir(os.getcwd())
    imlist=[filename for filename in allfiles if  filename[-4:] in [".jpg",".JPG"]]
    imlist = sorted(imlist)
    base_imlist = imlist

    WINDOWS = 10
    left = BASE_INDEX-WINDOWS
    right = BASE_INDEX+WINDOWS
    
    if(left < 0):
        left = 0
    
    if(right >= len(imlist)):
        right = len(imlist) - 1
    
    imlist = imlist[left:right]
    
    # Assuming all images are the same size, get dimensions of first image
    w,h=Image.open(imlist[0]).size
    N=len(imlist)

    # Create a numpy array of floats to store the average (assume RGB images)
    arr=numpy.zeros((h,w,3),numpy.float)

    # Build up average pixel intensities, casting each image as an array of floats
    images = map(lambda x: numpy.array(Image.open(x),dtype=numpy.float), imlist)

    def isWhite(col):
        ret = col[0] > 199 and col[1] > 199 and col[2] > 199
#        if (ret):
#            print col
        return ret

    i = BASE_INDEX
    print i, "/", len(imlist)
    for indx in xrange(len(images[i])):
        for indy in xrange(len(images[i][indx])):
            col = images[i][indx][indy]
            if(isWhite(col)):
                #images[i][indx][indy] = [255.0, 0.0, 0.0]
                #continue

                # Search for nearest full pixel
                dist = 1
                found = False
                while not found:
                    nind = i + dist
                    outOfBounds = False
                    if(nind >= len(imlist) ):
                        outOfBounds = True
                    elif(not isWhite(images[nind][indx][indy])):
                        #print "found in", nind
                        images[i][indx][indy] = images[nind][indx][indy]
                        found = True
                        break

                    nind = i - dist
                    if(nind < 0):
                        if(outOfBounds):
                            # both left and right bouds reached
                            #print "Not found"
                            #images[i][indx][indy] = [0.0, 0.0, 255.0]
                            break
                        else:
                            outOfBounds = True
                    elif(not isWhite(images[nind][indx][indy])):
                        #print "found in", nind
                        images[i][indx][indy] = images[nind][indx][indy]
                        found = True
                        break

                    dist += 1

    im = images[i]
    # Round values in array and cast as 8-bit integer
    arr=numpy.array(numpy.round(im),dtype=numpy.uint8)
    # Generate, save and preview final image
    out=Image.fromarray(arr,mode="RGB")
    out.save("corr/{0}.corr.png".format(base_imlist[i]))


allfiles = os.listdir(os.getcwd())
imlist = [filename for filename in allfiles if  filename[-4:] in [".jpg",".JPG"]]
imlist = sorted(imlist)

for i in xrange(len(imlist)):
    print i, "/", len(imlist)
    paging(i)