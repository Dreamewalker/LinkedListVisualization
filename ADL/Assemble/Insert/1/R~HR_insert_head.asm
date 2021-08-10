aLine 0
nNew newNodePtr, {0:D}

aLine 1
gBne Root, null, 7

aLine 2
gMove Rear, newNodePtr
nMoveAbs newNodePtr, 1285, 600

aLine 3
pSetNext newNodePtr, newNodePtr
Jmp 6

aLine 6
nMoveRelOut newNodePtr, Root, 190
pSetNext Rear, newNodePtr

aLine 7
pSetNext newNodePtr, Root

aLine 9
gMove Root, newNodePtr

aLine 10
aStd
gDelete newNodePtr
Halt