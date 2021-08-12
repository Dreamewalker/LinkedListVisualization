aLine 0
nNew newNodePtr, {0:D}
gNewVPtr temp
gMoveNext temp, Root
nMoveAbs newNodePtr, 1380, 435.455

aLine 1
gBne temp, null, 4

aLine 2
gMove Rear, newNodePtr
Jmp 3

aLine 5
pSetPrev temp, newNodePtr

aLine 7
pSetNext newNodePtr, temp

aLine 8
pSetNext Root, newNodePtr

aLine 9
pSetPrev newNodePtr, Root

aLine 10
aStd
gDelete newNodePtr
gDelete temp
Halt