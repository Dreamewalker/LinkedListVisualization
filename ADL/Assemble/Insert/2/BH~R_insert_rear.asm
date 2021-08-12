aLine 0
gNew currentPtr
gMove currentPtr, Root
gNewVPtr currentNext
gMoveNext currentNext, currentPtr

aLine 1
gBeq currentNext, null, 5

aLine 2
gMove currentPtr, currentNext
gMoveNext currentNext, currentNext
Jmp -5

aLine 4
nNew newNodePtr, {0:D}

aLine 5
nMoveRel newNodePtr, currentPtr, 90, -164.545
pSetNext currentPtr, newNodePtr

aLine 6
pSetPrev newNodePtr, currentPtr

aLine 7
aStd
gDelete newNodePtr
gDelete currentPtr
gDelete currentNext
Halt