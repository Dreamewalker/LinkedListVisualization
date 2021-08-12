aLine 0
nNew newNodePtr, {0:D}

aLine 1
gBne Root, null, 7

aLine 2
gMove Root, newNodePtr

aLine 3
gDelete newNodePtr
aStd
Halt

aLine 5
gNew currentPtr
gMove currentPtr, Root
gNewVPtr currentNext
gMoveNext currentNext, currentPtr

aLine 6
gBeq currentNext, null, 5

aLine 7
gMove currentPtr, currentNext
gMoveNext currentNext, currentNext
Jmp -5

aLine 9
nMoveRel newNodePtr, currentPtr, 95, -164.545
pSetNext currentPtr, newNodePtr

aLine 10
pSetPrev newNodePtr, currentPtr

aLine 11
aStd
gDelete newNodePtr
gDelete currentPtr
gDelete currentNext
Halt