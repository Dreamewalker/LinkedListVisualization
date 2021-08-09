aLine 0
nNew newNodePtr, {0:D}
gNewVPtr temp
gMoveNext temp, Root

aLine 1
gBne temp, null, 3

aLine 2
gMove Rear, newNodePtr

aLine 4
nMoveAbs newNodePtr, 1380, 435.455
pSetNext newNodePtr, temp

aLine 5
pSetNext Root, newNodePtr

aLine 6
aStd
gDelete newNodePtr
gDelete temp
Halt