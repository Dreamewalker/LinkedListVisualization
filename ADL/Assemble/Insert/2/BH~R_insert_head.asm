aLine 0
nNew newNodePtr, {0:D}
gNewVPtr temp
gMoveNext temp, Root
nMoveAbs newNodePtr, 1380, 435.455

aLine 1
gBeq temp, null, 3

aLine 2
pSetPrev temp, newNodePtr

aLine 4
pSetNext newNodePtr, temp

aLine 5
pSetNext Root, newNodePtr

aLine 6
pSetPrev newNodePtr, Root

aLine 7
aStd
gDelete newNodePtr
gDelete temp
Halt