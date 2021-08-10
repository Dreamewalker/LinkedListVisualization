aLine 0
nNew newNodePtr, {0:D}

aLine 1
gBne Root, null, 5

aLine 2
nMoveAbs newNodePtr, 1285, 600
pSetNext newNodePtr, newNodePtr
Jmp 19

aLine 5
gNew rearPtr
gMove rearPtr, Root
gNewVPtr rearNext
gMoveNext rearNext, rearPtr

aLine 6
gBeq rearNext, Root, 5

aLine 7
gMove rearPtr, rearNext
gMoveNext rearNext, rearNext
Jmp -5

aLine 9
nMoveRelOut newNodePtr, Root, 190
pSetNext rearPtr, newNodePtr

aLine 10
pSetNext newNodePtr, Root
gDelete rearPtr
gDelete rearNext

aLine 12
gMove Root, newNodePtr

aLine 13
aStd
gDelete newNodePtr
Halt