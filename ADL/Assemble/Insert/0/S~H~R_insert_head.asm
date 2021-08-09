aLine 0
nNew newNodePtr, {0}
nMoveAbs newNodePtr, 1095, 600

aLine 1
pSetNext newNodePtr, Root

aLine 2
gMove Root, newNodePtr

aLine 3
gDelete newNodePtr
aStd
Halt