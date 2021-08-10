aLine 0
nNew newNodePtr, {0:D}

aLine 1
gBne Root, null, 9

aLine 2
gMove Root, newNodePtr

aLine 3
pSetNext newNodePtr, newNodePtr

aLine 4
gDelete newNodePtr
aStd
Halt

aLine 6
gNew currentPtr
gMove currentPtr, Root
gNewVPtr currentNext
gMoveNext currentNext, currentPtr

aLine 7
gBeq currentNext, Root, 5

aLine 8
gMove currentPtr, currentNext
gMoveNext currentNext, currentNext
Jmp -5

aLine 10
nMoveRelOut newNodePtr, currentPtr, 190
pSetNext currentPtr, newNodePtr

aLine 11
pSetNext newNodePtr, Root

aLine 12
aStd
gDelete newNodePtr
gDelete currentPtr
gDelete currentNext
Halt