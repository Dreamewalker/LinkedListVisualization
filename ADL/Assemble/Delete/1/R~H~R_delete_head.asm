aLine 0
gBne Root, null, 3

aLine 1
Exception EMPTY_LIST

aLine 3
gNew rearPtr
gMove rearPtr, Root
gNewVPtr rearNext
gMoveNext rearNext, rearPtr

aLine 4
gBeq rearNext, Root, 5

aLine 5
gMove rearPtr, rearNext
gMoveNext rearNext, rearNext
Jmp -5

aLine 7
gNew delPtr
gMove delPtr, Root 

aLine 8
gBne Root, rearPtr, 4

aLine 9
gMove Root, null
Jmp 6

aLine 12
nMoveRelOut Root, Root, 100
gMoveNext Root, Root

aLine 13
pSetNext rearPtr, Root 

aLine 15
pDeleteNext delPtr
nDelete delPtr
gDelete delPtr
gDelete rearPtr
gDelete rearNext

aLine 16
aStd
Halt