aLine 0
gNew basePtr
gMove basePtr, Root
gNewVPtr maxNext
gNewVPtr rootNext
gMoveNext rootNext, Root

gNew maxPtr, 1085, 800
gNewVPtr maxPrev
gNew currentPtr, 1085, 920

aLine 1
gBeq basePtr, null, 42

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
gBeq maxPrev, null, 20

aLine 16
nMoveRel maxPtr, maxPtr, 0, -164.545
pSetNext maxPrev, maxNext

aLine 17
gBeq maxNext, null, 3

aLine 18
pSetPrev maxNext, maxPrev

aLine 20
pDeleteNext maxPtr
pDeletePrev maxPtr
nMoveRel maxPtr, Root, -95, -164.545
pSetNext maxPtr, Root

aLine 21
pSetPrev Root, maxPtr

aLine 22
gMove Root, maxPtr

aLine 23
pDeletePrev maxPtr
aStd

Jmp -42

aLine 26
gDelete basePtr
gDelete maxNext
gDelete maxPtr
gDelete maxPrev
gDelete rootNext
gDelete currentPtr
aStd
Halt