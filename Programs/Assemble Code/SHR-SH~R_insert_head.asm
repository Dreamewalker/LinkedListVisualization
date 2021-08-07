nNew newNodePtr, "{0}"
gNewVPtr temp

nMoveAbs newNodePtr, 1380, 435.455
gMoveNext temp, Root
nSetNextPtr newNodePtr, temp

nSetNextPtr Root, newNodePtr

aStd
gDelete newNodePtr
gDelete temp
Halt