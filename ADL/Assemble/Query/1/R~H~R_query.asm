aLine 0
gBne Root, null, 3

aLine 1
Exception NOT_FOUND

aLine 3
gNew currentPtr
gMove currentPtr, Root

aLine 4
sInit idx, 0
gNewVPtr currentNext
gMoveNext currentNext, currentPtr

aLine 5
gBeq currentNext, Root, 8
vBeq currentPtr, {0:D}, 7

aLine 6
gMove currentPtr, currentNext
gMoveNext currentNext, currentNext

aLine 7
sInc idx, 1
Jmp -8

aLine 9
gBne currentNext, Root, 3

aLine 10
Exception NOT_FOUND

aLine 12
gDelete currentPtr
gDelete currentNext
Yield idx