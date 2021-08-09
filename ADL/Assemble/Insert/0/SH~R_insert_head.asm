aLine 0
nNew newNodePtr, {0:D}
gNewVPtr temp

aLine 1
nMoveAbs newNodePtr, 1380, 435.455
gMoveNext temp, Root
pSetNext newNodePtr, temp

aLine 2
pSetNext Root, newNodePtr

aLine 3
aStd
gDelete newNodePtr
gDelete temp
Halt