aLine 0
nNew newNodePtr, {0}

aLine 1
nMoveRelOut newNodePtr, Rear, 190
pSetNext Rear, newNodePtr

aLine 2
gMove Rear, newNodePtr

aLine 3
pSetNext newNodePtr, Root

aLine 4
gDelete newNodePtr
aStd
Halt