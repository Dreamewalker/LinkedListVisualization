aLine 0
gNew basePtr
gMoveNext basePtr, Root
gNewVPtr minNext
gNewVPtr rootNext
gMoveNext rootNext, Root

aLine 1
gNew basePrev
gMove basePrev, Root
gNew minPtr, 1085, 800
gNew minPrev, 1085, 860
gNew currentPtr, 1085, 920
gNew currentPrev, 1085, 980

aLine 2
gBeq basePtr, null, 45

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
gBne basePrev, Root, 5

aLine 21
gMove basePrev, minPtr

aLine 22
gMove Rear, minPtr

aLine 24
gMoveNext minNext, minPtr
nMoveRel minPtr, minPtr, 0, -164.545
pSetNext minPrev, minNext

aLine 25
pDeleteNext minPtr
nMoveRel minPtr, Root, 95, -164.545
gMoveNext rootNext, Root
pSetNext minPtr, rootNext

aLine 26
pSetNext Root, minPtr
aStd

Jmp -45

aLine 28
gDelete basePrev
gDelete basePtr
gDelete minNext
gDelete minPtr
gDelete minPrev
gDelete rootNext
gDelete currentPtr
gDelete currentPrev
aStd
Halt