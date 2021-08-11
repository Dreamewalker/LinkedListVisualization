aLine 0
gNew currentPtr
gMoveNext currentPtr, Root

aLine 1
sInit idx, 0

aLine 2
gBeq currentPtr, Root, 7
vBeq currentPtr, {0:D}, 6

aLine 3
gMoveNext currentPtr, currentPtr

aLine 4
sInc idx, 1
Jmp -7

aLine 6
gBne currentPtr, Root, 3

aLine 7
Exception NOT_FOUND

aLine 9
gDelete currentPtr
Yield idx