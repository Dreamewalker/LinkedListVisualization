aLine 0
gNew basePtr
gMove basePtr, Root
gNewVPtr maxNext
gNewVPtr rootNext
gMoveNext rootNext, Root

gNewVPtr basePrev
gNew maxPtr, 1085, 800
gNewVPtr maxPrev
gNew currentPtr, 1085, 920

aLine 1
gBeq basePtr, null, 47

aLine 3
gMove maxPtr, basePtr

aLine 4
gMoveNext currentPtr, basePtr

aLine 5
gBeq currentPtr, null, 8

aLine 6
vBge maxPtr, currentPtr, 3

aLine 7
gMove maxPtr, currentPtr

aLine 9
gMoveNext currentPtr, currentPtr
Jmp -8

aLine 12
gMovePrev basePrev, basePtr
gBne basePrev, null, 3

aLine 13
gMove Rear, maxPtr

aLine 15
gBne maxPtr, basePtr, 3

aLine 16
gMoveNext basePtr, basePtr

aLine 18
gMovePrev maxPrev, maxPtr
gMoveNext maxNext, maxPtr
gBeq maxPrev, null, 20

aLine 19
nMoveRel maxPtr, maxPtr, 0, -164.545
pSetNext maxPrev, maxNext

aLine 20
gBeq maxNext, null, 3

aLine 21
pSetPrev maxNext, maxPrev

aLine 23
pDeleteNext maxPtr
pDeletePrev maxPtr
nMoveRel maxPtr, Root, -95, -164.545
pSetNext maxPtr, Root

aLine 24
pSetPrev Root, maxPtr

aLine 25
gMove Root, maxPtr

aLine 26
pDeletePrev maxPtr
aStd

Jmp -47

aLine 29
gDelete basePrev
gDelete basePtr
gDelete maxNext
gDelete maxPtr
gDelete maxPrev
gDelete rootNext
gDelete currentPtr
aStd
Halt