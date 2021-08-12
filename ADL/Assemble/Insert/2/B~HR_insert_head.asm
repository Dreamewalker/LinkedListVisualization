aLine 0
nNew newNodePtr, {0}

aLine 1
gBne Root, null, 4

aLine 2
gMove Rear, newNodePtr
Jmp 3

aLine 5
pSetPrev Root, newNodePtr

aLine 7
nMoveAbs newNodePtr, 1095, 600
pSetNext newNodePtr, Root

aLine 8
gMove Root, newNodePtr

aLine 9
gDelete newNodePtr
aStd
Halt