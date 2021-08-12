aLine 0
gNew basePtr
gMoveNext basePtr, Root
gNewVPtr minNext
gNewVPtr rootNext
gMoveNext rootNext, Root

gNewVPtr basePrev
gNew minPtr, 1085, 800
gNewVPtr minPrev
gNew currentPtr, 1085, 920

aLine 1
gBeq basePtr, null, 41

aLine 3
gMove minPtr, basePtr

aLine 4
gMoveNext currentPtr, basePtr

aLine 5
gBeq currentPtr, null, 8

aLine 6
vBle minPtr, currentPtr, 3

aLine 7
gMove minPtr, currentPtr

aLine 9
gMoveNext currentPtr, currentPtr
Jmp -8

aLine 12
gBne minPtr, basePtr, 3

aLine 13
gMoveNext basePtr, basePtr

aLine 15
gMovePrev minPrev, minPtr
gMoveNext minNext, minPtr
nMoveRel minPtr, minPtr, 0, -164.545
pSetNext minPrev, minNext

aLine 16
gBeq minNext, null, 3

aLine 17
pSetPrev minNext, minPrev

aLine 19
pDeleteNext minPtr
pDeletePrev minPtr
nMoveRel minPtr, Root, 95, -164.545
gMoveNext rootNext, Root
pSetNext minPtr, rootNext

aLine 20
pSetPrev rootNext, minPtr

aLine 21
pSetNext Root, minPtr

aLine 22
pSetPrev minPtr, Root
aStd

Jmp -41

aLine 24
gDelete basePrev
gDelete basePtr
gDelete minNext
gDelete minPtr
gDelete minPrev
gDelete rootNext
gDelete currentPtr
aStd
Halt