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
gBeq basePtr, null, 41

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
gBne maxPtr, basePtr, 3

aLine 13
gMoveNext basePtr, basePtr

aLine 15
gMovePrev maxPrev, maxPtr
gMoveNext maxNext, maxPtr
nMoveRel maxPtr, maxPtr, 0, -164.545
pSetNext maxPrev, maxNext

aLine 16
gBeq maxNext, null, 3

aLine 17
pSetPrev maxNext, maxPrev

aLine 19
pDeleteNext maxPtr
pDeletePrev maxPtr
nMoveRel maxPtr, Root, 95, -164.545
gMoveNext rootNext, Root
pSetNext maxPtr, rootNext

aLine 20
pSetPrev rootNext, maxPtr

aLine 21
pSetNext Root, maxPtr

aLine 22
pSetPrev maxPtr, Root
aStd

Jmp -41

aLine 24
gDelete basePrev
gDelete basePtr
gDelete maxNext
gDelete maxPtr
gDelete maxPrev
gDelete rootNext
gDelete currentPtr
aStd
Halt