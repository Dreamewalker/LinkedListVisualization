aLine 0
nNew newNodePtr, {0}

aLine 1
nMoveRel newNodePtr, Rear, 95, -164.545
pSetNext Rear, newNodePtr

aLine 2
gMove Rear, newNodePtr

aLine 3
gDelete newNodePtr
aStd
Halt