aLine 0
gNew currentPtr
gMove currentPtr, Root
gNewVPtr currentNext
gMoveNext currentNext, currentPtr

aLine 1
gBeq currentNext, Root, 5

aLine 2
gMove currentPtr, currentNext
gMoveNext currentNext, currentNext
Jmp -5

aLine 4
nNew newNodePtr, {0:D}

aLine 5
nMoveRelOut newNodePtr, currentPtr, 190
pSetNext currentPtr, newNodePtr

aLine 6
pSetNext newNodePtr, Root

aLine 7
aStd
gDelete newNodePtr
gDelete currentPtr
gDelete currentNext
Halt