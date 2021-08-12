aLine 0
gNew basePtr
gMove basePtr, Root
gNewVPtr minNext
gNewVPtr rootNext
gMoveNext rootNext, Root

gNewVPtr basePrev
gNew minPtr, 1085, 800
gNewVPtr minPrev
gNew currentPtr, 1085, 920

aLine 1
gBeq basePtr, null, 47

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
gMovePrev basePrev, basePtr
gBne basePrev, null, 3

aLine 13
gMove Rear, minPtr

aLine 15
gBne minPtr, basePtr, 3

aLine 16
gMoveNext basePtr, basePtr

aLine 18
gMovePrev minPrev, minPtr
gMoveNext minNext, minPtr
gBeq minPrev, null, 20

aLine 19
nMoveRel minPtr, minPtr, 0, -164.545
pSetNext minPrev, minNext

aLine 20
gBeq minNext, null, 3

aLine 21
pSetPrev minNext, minPrev

aLine 23
pDeleteNext minPtr
pDeletePrev minPtr
nMoveRel minPtr, Root, -95, -164.545
pSetNext minPtr, Root

aLine 24
pSetPrev Root, minPtr

aLine 25
gMove Root, minPtr

aLine 26
pDeletePrev minPtr
aStd

Jmp -47

aLine 29
gDelete basePrev
gDelete basePtr
gDelete minNext
gDelete minPtr
gDelete minPrev
gDelete rootNext
gDelete currentPtr
aStd
Halt