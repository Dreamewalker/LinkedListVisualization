aLine 0
gNew basePtr
gMove basePtr, Root
gNewVPtr minNext

aLine 1
gNew basePrev
gMove basePrev, null
gNew minPtr, 1085, 800
gNew minPrev, 1085, 860
gNew currentPtr, 1085, 920
gNew currentPrev, 1085, 980

aLine 2
gBeq basePtr, null, 46

aLine 4
gMove minPtr, basePtr

aLine 5
gMove minPrev, basePrev

aLine 6
gMoveNext currentPtr, basePtr

aLine 7
gMove currentPrev, basePtr

aLine 8
gBeq currentPtr, null, 12

aLine 9
vBle minPtr, currentPtr, 5

aLine 10
gMove minPrev, currentPrev

aLine 11
gMove minPtr, currentPtr

aLine 13
gMove currentPrev, currentPtr

aLine 14
gMoveNext currentPtr, currentPtr

Jmp -12

aLine 17
gBne minPtr, basePtr, 3

aLine 18
gMoveNext basePtr, basePtr

aLine 20
gBne basePrev, null, 5

aLine 21
gMove basePrev, minPtr

aLine 22
gMove Rear, minPtr

aLine 24
gBeq minPrev, null, 12

aLine 25
gMoveNext minNext, minPtr
nMoveRel minPtr, minPtr, 0, -164.545
pSetNext minPrev, minNext

aLine 26
pDeleteNext minPtr
nMoveRel minPtr, Root, -95, -164.545
pSetNext minPtr, Root

aLine 27
gMove Root, minPtr
aStd

Jmp -46

aLine 30
gDelete basePrev
gDelete basePtr
gDelete minNext
gDelete minPtr
gDelete minPrev
gDelete currentPtr
gDelete currentPrev
aStd
Halt