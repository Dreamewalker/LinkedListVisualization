aLine 0
nNew newNodePtr, {0}

aLine 1
gBeq Root, null, 3

aLine 2
pSetPrev Root, newNodePtr

aLine 4
nMoveAbs newNodePtr, 1095, 600
pSetNext newNodePtr, Root

aLine 5
gMove Root, newNodePtr

aLine 6
gDelete newNodePtr
aStd
Halt