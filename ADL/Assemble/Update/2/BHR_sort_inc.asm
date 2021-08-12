aLine 0
gNew basePtr
gMoveNext basePtr, Root
gNewVPtr maxNext
gNewVPtr rootNext
gMoveNext rootNext, Root

gNewVPtr basePrev
gNew maxPtr, 1085, 800
gNewVPtr maxPrev
gNew currentPtr, 1085, 920

aLine 1
gBeq basePtr, null, 46

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
gBne basePrev, Root, 3

aLine 13
gMove Rear, maxPtr

aLine 15
gBne maxPtr, basePtr, 3

aLine 16
gMoveNext basePtr, basePtr

aLine 18
gMovePrev maxPrev, maxPtr
gMoveNext maxNext, maxPtr
nMoveRel maxPtr, maxPtr, 0, -164.545
pSetNext maxPrev, maxNext

aLine 19
gBeq maxNext, null, 3

aLine 20
pSetPrev maxNext, maxPrev

aLine 22
pDeleteNext maxPtr
pDeletePrev maxPtr
nMoveRel maxPtr, Root, 95, -164.545
gMoveNext rootNext, Root
pSetNext maxPtr, rootNext

aLine 23
pSetPrev rootNext, maxPtr

aLine 24
pSetNext Root, maxPtr

aLine 25
pSetPrev maxPtr, Root
aStd

Jmp -46

aLine 27
gDelete basePrev
gDelete basePtr
gDelete maxNext
gDelete maxPtr
gDelete maxPrev
gDelete rootNext
gDelete currentPtr
aStd
Halt