aLine 0
gNew delPtr
gMoveNext delPtr, Root

aLine 1
gBne delPtr, Root, 3

aLine 2
Exception EMPTY_LIST

aLine 4
nMoveRelOut delPtr, delPtr, 100
gNewVPtr delNext
gMoveNext delNext, delPtr
pSetNext Root, delNext

aLine 5
pDeleteNext delPtr
nDelete delPtr
gDelete delPtr
gDelete delNext

aLine 6
aStd
Halt