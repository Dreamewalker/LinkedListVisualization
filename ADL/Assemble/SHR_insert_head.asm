aLine 0
nNew newNodePtr, {0}
gNewVPtr temp

aLine 1
nMoveAbs newNodePtr, 1380, 435.455
gMoveNext temp, Root
nSetNextPtr newNodePtr, temp

aLine 2
nSetNextPtr Root, newNodePtr

aLine 3
aStd
gDelete newNodePtr
gDelete temp
Halt