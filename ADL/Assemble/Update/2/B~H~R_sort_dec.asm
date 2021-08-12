aLine 0
gNew basePtr
gMove basePtr, Root
gNewVPtr minNext
gNewVPtr rootNext
gMoveNext rootNext, Root

gNew minPtr, 1085, 800
gNewVPtr minPrev
gNew currentPtr, 1085, 920

aLine 1
gBeq basePtr, null, 42

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
gBeq minPrev, null, 20

aLine 16
nMoveRel minPtr, minPtr, 0, -164.545
pSetNext minPrev, minNext

aLine 17
gBeq minNext, null, 3

aLine 18
pSetPrev minNext, minPrev

aLine 20
pDeleteNext minPtr
pDeletePrev minPtr
nMoveRel minPtr, Root, -95, -164.545
pSetNext minPtr, Root

aLine 21
pSetPrev Root, minPtr

aLine 22
gMove Root, minPtr

aLine 23
pDeletePrev minPtr
aStd

Jmp -42

aLine 26
gDelete basePtr
gDelete minNext
gDelete minPtr
gDelete minPrev
gDelete rootNext
gDelete currentPtr
aStd
Halt