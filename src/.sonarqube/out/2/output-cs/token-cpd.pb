գ
YC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Services\UserService.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Services '
{ 
public 

class 
UserService 
: 
IUserService +
{ 
private 
readonly 
IUnitOfWork $
_unitOfWork% 0
;0 1
private 
readonly 
IMapper  
_mapper! (
;( )
private 
readonly 
ILoggerManager '
<' (
UserService( 3
>3 4
_logger5 <
;< =
private 
readonly 
ICurrentUserService ,
_currentUserService- @
;@ A
public!! 
UserService!! 
(!! 
IUnitOfWork"" 

unitOfWork"" "
,""" #
IMapper## 
mapper## 
,## 
ILoggerManager$$ 
<$$ 
UserService$$ &
>$$& '
logger$$( .
,$$. /
ICurrentUserService%% 
currentUserService%%  2
)%%2 3
{&& 	
_unitOfWork'' 
='' 

unitOfWork'' $
;''$ %
_mapper(( 
=(( 
mapper(( 
;(( 
_logger)) 
=)) 
logger)) 
;)) 
_currentUserService** 
=**  !
currentUserService**" 4
;**4 5
}++ 	
public11 
async11 
Task11 
<11 
IEnumerable11 %
<11% &
User11& *
>11* +
>11+ ,
GetUsersAsync11- :
(11: ;
)11; <
{22 	
try33 
{44 
_logger55 
.55 
LogInfo55 
(55  
LogMessages55  +
.55+ ,
GettingAllUsers55, ;
)55; <
;55< =
var66 
spec66 
=66 
new66 
AllSpecification66 /
<66/ 0
User660 4
>664 5
(665 6
)666 7
;667 8
var77 
users77 
=77 
await77 !
_unitOfWork77" -
.77- .
Users77. 3
.773 4
	ListAsync774 =
(77= >
spec77> B
)77B C
;77C D
_logger88 
.88 
LogInfo88 
(88  
LogMessages88  +
.88+ ,
UsersRetrieved88, :
,88: ;
users88< A
.88A B
Count88B G
)88G H
;88H I
return99 
users99 
;99 
}:: 
catch;; 
(;; 
	Exception;; 
ex;; 
);;  
{<< 
_logger== 
.== 
LogError==  
(==  !
ex==! #
,==# $
LogMessages==% 0
.==0 1
ErrorGettingUsers==1 B
)==B C
;==C D
throw>> 
;>> 
}?? 
}@@ 	
publicGG 
asyncGG 
TaskGG 
<GG 
UserGG 
?GG 
>GG  
GetUserByIdAsyncGG! 1
(GG1 2
intGG2 5
idGG6 8
)GG8 9
{HH 	
tryII 
{JJ 
_loggerKK 
.KK 
LogInfoKK 
(KK  
LogMessagesKK  +
.KK+ ,
GettingUserByIdKK, ;
,KK; <
idKK= ?
)KK? @
;KK@ A
varLL 
specLL 
=LL 
newLL #
EntityByIdSpecificationLL 6
<LL6 7
UserLL7 ;
>LL; <
(LL< =
idLL= ?
)LL? @
;LL@ A
varMM 
userMM 
=MM 
awaitMM  
_unitOfWorkMM! ,
.MM, -
UsersMM- 2
.MM2 3
FirstOrDefaultAsyncMM3 F
(MMF G
specMMG K
)MMK L
;MML M
ifOO 
(OO 
userOO 
==OO 
nullOO  
)OO  !
{PP 
_loggerQQ 
.QQ 
LogWarnQQ #
(QQ# $
LogMessagesQQ$ /
.QQ/ 0
UserNotFoundByIdQQ0 @
,QQ@ A
idQQB D
)QQD E
;QQE F
returnRR 
nullRR 
;RR  
}SS 
_loggerUU 
.UU 
LogInfoUU 
(UU  
LogMessagesUU  +
.UU+ ,
UserRetrievedUU, 9
,UU9 :
idUU; =
)UU= >
;UU> ?
returnVV 
userVV 
;VV 
}WW 
catchXX 
(XX 
	ExceptionXX 
exXX 
)XX  
{YY 
_loggerZZ 
.ZZ 
LogErrorZZ  
(ZZ  !
exZZ! #
,ZZ# $
LogMessagesZZ% 0
.ZZ0 1 
ErrorGettingUserByIdZZ1 E
,ZZE F
idZZG I
)ZZI J
;ZZJ K
throw[[ 
;[[ 
}\\ 
}]] 	
publicdd 
asyncdd 
Taskdd 
<dd 
Userdd 
>dd 
CreateUserAsyncdd  /
(dd/ 0
Userdd0 4
userdd5 9
)dd9 :
{ee 	
tryff 
{gg 
_loggerhh 
.hh 
LogInfohh 
(hh  
LogMessageshh  +
.hh+ ,!
CreatingUserWithSicilhh, A
,hhA B
userhhC G
.hhG H
SicilhhH M
)hhM N
;hhN O
varjj !
existingUserSpecSiciljj )
=jj* +
newjj, /$
UserBySicilSpecificationjj0 H
(jjH I
userjjI M
.jjM N
SiciljjN S
,jjS T
includeRolejjU `
:jj` a
falsejjb g
)jjg h
;jjh i
varkk 
existingUserBySicilkk '
=kk( )
awaitkk* /
_unitOfWorkkk0 ;
.kk; <
Userskk< A
.kkA B
FirstOrDefaultAsynckkB U
(kkU V!
existingUserSpecSicilkkV k
)kkk l
;kkl m
ifmm 
(mm 
existingUserBySicilmm '
!=mm( *
nullmm+ /
)mm/ 0
{nn 
_loggeroo 
.oo 
LogWarnoo #
(oo# $
LogMessagesoo$ /
.oo/ 0
DuplicateSiciloo0 >
,oo> ?
useroo@ D
.ooD E
SicilooE J
)ooJ K
;ooK L
throwpp 
newpp %
InvalidOperationExceptionpp 7
(pp7 8
ErrorMessagespp8 E
.ppE F
DuplicateSicilppF T
)ppT U
;ppU V
}qq 
userss 
.ss 
	CreatedAtss 
=ss  
DateTimess! )
.ss) *
UtcNowss* 0
;ss0 1
awaittt 
_unitOfWorktt !
.tt! "
Userstt" '
.tt' (
AddAsynctt( 0
(tt0 1
usertt1 5
)tt5 6
;tt6 7
awaituu 
_unitOfWorkuu !
.uu! "
SaveChangesAsyncuu" 2
(uu2 3
)uu3 4
;uu4 5
_loggerww 
.ww 
LogInfoww 
(ww  
LogMessagesww  +
.ww+ , 
UserCreatedWithSicilww, @
,ww@ A
userwwB F
.wwF G
IdwwG I
,wwI J
userwwK O
.wwO P
SicilwwP U
)wwU V
;wwV W
returnxx 
userxx 
;xx 
}yy 
catchzz 
(zz 
	Exceptionzz 
exzz 
)zz  
{{{ 
_logger|| 
.|| 
LogError||  
(||  !
ex||! #
,||# $
LogMessages||% 0
.||0 1&
ErrorCreatingUserWithSicil||1 K
,||K L
user||M Q
.||Q R
Sicil||R W
)||W X
;||X Y
throw}} 
;}} 
}~~ 
} 	
public
�� 
async
�� 
Task
�� 
<
�� 
User
�� 
>
�� 
UpdateUserAsync
��  /
(
��/ 0
int
��0 3
id
��4 6
,
��6 7
User
��8 <
user
��= A
)
��A B
{
�� 	
try
�� 
{
�� 
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,
UpdatingUser
��, 8
,
��8 9
id
��: <
)
��< =
;
��= >
var
�� 
existingUserSpec
�� $
=
��% &
new
��' *%
EntityByIdSpecification
��+ B
<
��B C
User
��C G
>
��G H
(
��H I
id
��I K
)
��K L
;
��L M
var
�� 
existingUser
��  
=
��! "
await
��# (
_unitOfWork
��) 4
.
��4 5
Users
��5 :
.
��: ;!
FirstOrDefaultAsync
��; N
(
��N O
existingUserSpec
��O _
)
��_ `
;
��` a
if
�� 
(
�� 
existingUser
��  
==
��! #
null
��$ (
)
��( )
{
�� 
_logger
�� 
.
�� 
LogWarn
�� #
(
��# $
LogMessages
��$ /
.
��/ 0
UserNotFoundById
��0 @
,
��@ A
id
��B D
)
��D E
;
��E F
throw
�� 
new
�� '
InvalidOperationException
�� 7
(
��7 8
ErrorMessages
��8 E
.
��E F
UserNotFound
��F R
)
��R S
;
��S T
}
�� 
if
�� 
(
�� 
user
�� 
.
�� 
Sicil
�� 
!=
�� !
existingUser
��" .
.
��. /
Sicil
��/ 4
)
��4 5
{
�� 
var
�� 
duplicateUserSpec
�� )
=
��* +
new
��, /&
UserBySicilSpecification
��0 H
(
��H I
user
��I M
.
��M N
Sicil
��N S
,
��S T
includeRole
��U `
:
��` a
false
��b g
)
��g h
;
��h i
var
�� 
duplicateUser
�� %
=
��& '
await
��( -
_unitOfWork
��. 9
.
��9 :
Users
��: ?
.
��? @!
FirstOrDefaultAsync
��@ S
(
��S T
duplicateUserSpec
��T e
)
��e f
;
��f g
if
�� 
(
�� 
duplicateUser
�� %
!=
��& (
null
��) -
&&
��. 0
duplicateUser
��1 >
.
��> ?
Id
��? A
!=
��B D
id
��E G
)
��G H
{
�� 
_logger
�� 
.
��  
LogWarn
��  '
(
��' (
LogMessages
��( 3
.
��3 4
DuplicateSicil
��4 B
,
��B C
user
��D H
.
��H I
Sicil
��I N
)
��N O
;
��O P
throw
�� 
new
�� !'
InvalidOperationException
��" ;
(
��; <
ErrorMessages
��< I
.
��I J
DuplicateSicil
��J X
)
��X Y
;
��Y Z
}
�� 
}
�� 
existingUser
�� 
.
�� 
Sicil
�� "
=
��# $
user
��% )
.
��) *
Sicil
��* /
;
��/ 0
existingUser
�� 
.
�� 
Adi
��  
=
��! "
user
��# '
.
��' (
Adi
��( +
;
��+ ,
existingUser
�� 
.
�� 
Soyadi
�� #
=
��$ %
user
��& *
.
��* +
Soyadi
��+ 1
;
��1 2
existingUser
�� 
.
�� 
IsAdmin
�� $
=
��% &
user
��' +
.
��+ ,
IsAdmin
��, 3
;
��3 4
existingUser
�� 
.
�� 
RoleId
�� #
=
��$ %
user
��& *
.
��* +
RoleId
��+ 1
;
��1 2
existingUser
�� 
.
�� 
	UpdatedAt
�� &
=
��' (
DateTime
��) 1
.
��1 2
UtcNow
��2 8
;
��8 9
_unitOfWork
�� 
.
�� 
Users
�� !
.
��! "
Update
��" (
(
��( )
existingUser
��) 5
)
��5 6
;
��6 7
await
�� 
_unitOfWork
�� !
.
��! "
SaveChangesAsync
��" 2
(
��2 3
)
��3 4
;
��4 5
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,
UserUpdated
��, 7
,
��7 8
id
��9 ;
)
��; <
;
��< =
return
�� 
existingUser
�� #
;
��# $
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
_logger
�� 
.
�� 
LogError
��  
(
��  !
ex
��! #
,
��# $
LogMessages
��% 0
.
��0 1
ErrorUpdatingUser
��1 B
,
��B C
id
��D F
)
��F G
;
��G H
throw
�� 
;
�� 
}
�� 
}
�� 	
public
�� 
async
�� 
Task
�� 
<
�� 
bool
�� 
>
�� 
DeleteUserAsync
��  /
(
��/ 0
int
��0 3
id
��4 6
)
��6 7
{
�� 	
try
�� 
{
�� 
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,
DeletingUser
��, 8
,
��8 9
id
��: <
)
��< =
;
��= >
var
�� 
userSpec
�� 
=
�� 
new
�� "%
EntityByIdSpecification
��# :
<
��: ;
User
��; ?
>
��? @
(
��@ A
id
��A C
)
��C D
;
��D E
var
�� 
user
�� 
=
�� 
await
��  
_unitOfWork
��! ,
.
��, -
Users
��- 2
.
��2 3!
FirstOrDefaultAsync
��3 F
(
��F G
userSpec
��G O
)
��O P
;
��P Q
if
�� 
(
�� 
user
�� 
==
�� 
null
��  
)
��  !
{
�� 
_logger
�� 
.
�� 
LogWarn
�� #
(
��# $
LogMessages
��$ /
.
��/ 0
UserNotFoundById
��0 @
,
��@ A
id
��B D
)
��D E
;
��E F
return
�� 
false
��  
;
��  !
}
�� 
if
�� 
(
�� 
user
�� 
.
�� 
IsAdmin
��  
)
��  !
{
�� 
_logger
�� 
.
�� 
LogWarn
�� #
(
��# $
LogMessages
��$ /
.
��/ 0#
CannotDeleteAdminUser
��0 E
,
��E F
id
��G I
)
��I J
;
��J K
throw
�� 
new
�� '
InvalidOperationException
�� 7
(
��7 8
ErrorMessages
��8 E
.
��E F#
CannotDeleteAdminUser
��F [
)
��[ \
;
��\ ]
}
�� 
if
�� 
(
�� !
_currentUserService
�� '
.
��' (
UserId
��( .
==
��/ 1
id
��2 4
)
��4 5
{
�� 
_logger
�� 
.
�� 
LogWarn
�� #
(
��# $
LogMessages
��$ /
.
��/ 0
CannotDeleteSelf
��0 @
,
��@ A
id
��B D
)
��D E
;
��E F
throw
�� 
new
�� '
InvalidOperationException
�� 7
(
��7 8
ErrorMessages
��8 E
.
��E F
CannotDeleteSelf
��F V
)
��V W
;
��W X
}
�� 
_unitOfWork
�� 
.
�� 
Users
�� !
.
��! "
Delete
��" (
(
��( )
user
��) -
)
��- .
;
��. /
await
�� 
_unitOfWork
�� !
.
��! "
SaveChangesAsync
��" 2
(
��2 3
)
��3 4
;
��4 5
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,
UserDeleted
��, 7
,
��7 8
id
��9 ;
)
��; <
;
��< =
return
�� 
true
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
_logger
�� 
.
�� 
LogError
��  
(
��  !
ex
��! #
,
��# $
LogMessages
��% 0
.
��0 1
ErrorDeletingUser
��1 B
,
��B C
id
��D F
)
��F G
;
��G H
throw
�� 
;
�� 
}
�� 
}
�� 	
public
�� 
async
�� 
Task
�� 
<
�� 
bool
�� 
>
�� !
UpdateUserRoleAsync
��  3
(
��3 4
int
��4 7
userId
��8 >
,
��> ?
int
��@ C
roleId
��D J
)
��J K
{
�� 	
try
�� 
{
�� 
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,
UpdatingUserRole
��, <
,
��< =
userId
��> D
,
��D E
roleId
��F L
)
��L M
;
��M N
var
�� 
userSpec
�� 
=
�� 
new
�� "%
EntityByIdSpecification
��# :
<
��: ;
User
��; ?
>
��? @
(
��@ A
userId
��A G
)
��G H
;
��H I
var
�� 
user
�� 
=
�� 
await
��  
_unitOfWork
��! ,
.
��, -
Users
��- 2
.
��2 3!
FirstOrDefaultAsync
��3 F
(
��F G
userSpec
��G O
)
��O P
;
��P Q
if
�� 
(
�� 
user
�� 
==
�� 
null
��  
)
��  !
{
�� 
_logger
�� 
.
�� 
LogWarn
�� #
(
��# $
LogMessages
��$ /
.
��/ 0
UserNotFoundById
��0 @
,
��@ A
userId
��B H
)
��H I
;
��I J
return
�� 
false
��  
;
��  !
}
�� 
var
�� 
roleSpec
�� 
=
�� 
new
�� "%
EntityByIdSpecification
��# :
<
��: ;
Role
��; ?
>
��? @
(
��@ A
roleId
��A G
)
��G H
;
��H I
var
�� 
role
�� 
=
�� 
await
��  
_unitOfWork
��! ,
.
��, -
Roles
��- 2
.
��2 3!
FirstOrDefaultAsync
��3 F
(
��F G
roleSpec
��G O
)
��O P
;
��P Q
if
�� 
(
�� 
role
�� 
==
�� 
null
��  
)
��  !
{
�� 
_logger
�� 
.
�� 
LogWarn
�� #
(
��# $
LogMessages
��$ /
.
��/ 0
RoleNotFoundById
��0 @
,
��@ A
roleId
��B H
)
��H I
;
��I J
return
�� 
false
��  
;
��  !
}
�� 
user
�� 
.
�� 
RoleId
�� 
=
�� 
roleId
�� $
;
��$ %
user
�� 
.
�� 
	UpdatedAt
�� 
=
��  
DateTime
��! )
.
��) *
UtcNow
��* 0
;
��0 1
_unitOfWork
�� 
.
�� 
Users
�� !
.
��! "
Update
��" (
(
��( )
user
��) -
)
��- .
;
��. /
await
�� 
_unitOfWork
�� !
.
��! "
SaveChangesAsync
��" 2
(
��2 3
)
��3 4
;
��4 5
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,
UserRoleUpdated
��, ;
,
��; <
userId
��= C
,
��C D
roleId
��E K
)
��K L
;
��L M
return
�� 
true
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
_logger
�� 
.
�� 
LogError
��  
(
��  !
ex
��! #
,
��# $
LogMessages
��% 0
.
��0 1#
ErrorUpdatingUserRole
��1 F
,
��F G
userId
��H N
,
��N O
roleId
��P V
)
��V W
;
��W X
throw
�� 
;
�� 
}
�� 
}
�� 	
}
�� 
}�� �;
cC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Services\UserPermissionService.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Services '
{ 
public 

class !
UserPermissionService &
:' ("
IUserPermissionService) ?
{ 
private 
readonly 
IUnitOfWork $
_unitOfWork% 0
;0 1
private 
readonly 
ICurrentUserService ,
_currentUserService- @
;@ A
private 
readonly 
ILoggerManager '
<' (!
UserPermissionService( =
>= >
_logger? F
;F G
private 
readonly 
IPermissionService +
_permissionService, >
;> ?
public !
UserPermissionService $
($ %
IUnitOfWork 

unitOfWork "
," #
ICurrentUserService 
currentUserService  2
,2 3
ILoggerManager 
< !
UserPermissionService 0
>0 1
logger2 8
,8 9
IPermissionService 
permissionService 0
)0 1
{ 	
_unitOfWork 
= 

unitOfWork $
;$ %
_currentUserService   
=    !
currentUserService  " 4
;  4 5
_logger!! 
=!! 
logger!! 
;!! 
_permissionService"" 
=""  
permissionService""! 2
;""2 3
}## 	
public%% 
async%% 
Task%% 
<%% 
List%% 
<%% 
PermissionDto%% ,
>%%, -
>%%- .#
GetUserPermissionsAsync%%/ F
(%%F G
int%%G J
userId%%K Q
)%%Q R
{&& 	
return'' 
await'' 
_permissionService'' +
.''+ ,'
GetPermissionsByUserIdAsync'', G
(''G H
userId''H N
)''N O
;''O P
}(( 	
public** 
async** 
Task** 
<** 
bool** 
>** '
AssignPermissionToUserAsync**  ;
(**; <
int**< ?
userId**@ F
,**F G
int**H K
permissionId**L X
,**X Y
bool**Z ^
	isGranted**_ h
)**h i
{++ 	
return,, 
await,, 
_permissionService,, +
.,,+ ,'
AssignPermissionToUserAsync,,, G
(,,G H
userId,,H N
,,,N O
permissionId,,P \
,,,\ ]
	isGranted,,^ g
),,g h
;,,h i
}-- 	
public// 
async// 
Task// 
<// 
bool// 
>// %
RemoveUserPermissionAsync//  9
(//9 :
int//: =
userId//> D
,//D E
int//F I
permissionId//J V
)//V W
{00 	
return11 
await11 
_permissionService11 +
.11+ ,%
RemoveUserPermissionAsync11, E
(11E F
userId11F L
,11L M
permissionId11N Z
)11Z [
;11[ \
}22 	
public44 
async44 
Task44 
<44 
bool44 
>44 
HasPermissionAsync44  2
(442 3
int443 6
userId447 =
,44= >
string44? E
permissionName44F T
)44T U
{55 	
return66 
await66 
_permissionService66 +
.66+ ,"
UserHasPermissionAsync66, B
(66B C
userId66C I
,66I J
permissionName66K Y
)66Y Z
;66Z [
}77 	
public99 
async99 
Task99 
<99 
List99 
<99 
PermissionDto99 ,
>99, -
>99- .)
GetUserCustomPermissionsAsync99/ L
(99L M
int99M P
userId99Q W
)99W X
{:: 	
try;; 
{<< 
var== 
userPermissionRepo== &
===' (
_unitOfWork==) 4
.==4 5
GetRepository==5 B
<==B C
UserPermission==C Q
>==Q R
(==R S
)==S T
;==T U
var>> 
spec>> 
=>> 
new>> .
"UserCustomPermissionsSpecification>> A
(>>A B
userId>>B H
)>>H I
;>>I J
var?? 
userPermissions?? #
=??$ %
await??& +
userPermissionRepo??, >
.??> ?
	ListAsync??? H
(??H I
spec??I M
)??M N
;??N O
returnAA 
userPermissionsAA &
.AA& '
SelectAA' -
(AA- .
upAA. 0
=>AA1 3
newAA4 7
PermissionDtoAA8 E
{BB 
IdCC 
=CC 
upCC 
.CC 
PermissionIdCC (
,CC( )
	IsGrantedDD 
=DD 
upDD  "
.DD" #
	IsGrantedDD# ,
,DD, -
NameEE 
=EE 
upEE 
.EE 

PermissionEE (
?EE( )
.EE) *
NameEE* .
??EE/ 1
stringEE2 8
.EE8 9
EmptyEE9 >
,EE> ?
ResourceTypeFF  
=FF! "
upFF# %
.FF% &

PermissionFF& 0
?FF0 1
.FF1 2
ResourceTypeFF2 >
??FF? A
stringFFB H
.FFH I
EmptyFFI N
,FFN O
DescriptionGG 
=GG  !
upGG" $
.GG$ %

PermissionGG% /
?GG/ 0
.GG0 1
DescriptionGG1 <
??GG= ?
stringGG@ F
.GGF G
EmptyGGG L
,GGL M
GroupHH 
=HH 
upHH 
.HH 

PermissionHH )
?HH) *
.HH* +
GroupHH+ 0
??HH1 3
stringHH4 :
.HH: ;
EmptyHH; @
,HH@ A
IsCustomII 
=II 
trueII #
}JJ 
)JJ 
.JJ 
OrderByJJ 
(JJ 
pJJ 
=>JJ 
pJJ  !
.JJ! "
GroupJJ" '
)JJ' (
.JJ( )
ThenByJJ) /
(JJ/ 0
pJJ0 1
=>JJ2 4
pJJ5 6
.JJ6 7
NameJJ7 ;
)JJ; <
.JJ< =
ToListJJ= C
(JJC D
)JJD E
;JJE F
}KK 
catchLL 
(LL 
	ExceptionLL 
exLL 
)LL  
{MM 
_loggerNN 
.NN 
LogErrorNN  
(NN  !
exNN! #
,NN# $
LogMessagesNN% 0
.NN0 1-
!ErrorGettingCustomUserPermissionsNN1 R
,NNR S
userIdNNT Z
)NNZ [
;NN[ \
returnOO 
newOO 
ListOO 
<OO  
PermissionDtoOO  -
>OO- .
(OO. /
)OO/ 0
;OO0 1
}PP 
}QQ 	
publicSS 
asyncSS 
TaskSS 
<SS 
boolSS 
>SS +
ResetUserToRolePermissionsAsyncSS  ?
(SS? @
intSS@ C
userIdSSD J
)SSJ K
{TT 	
returnUU 
awaitUU 
_permissionServiceUU +
.UU+ ,+
ResetUserToRolePermissionsAsyncUU, K
(UUK L
userIdUUL R
)UUR S
;UUS T
}VV 	
}WW 
}XX �
_C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Services\PermissionService.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Services '
{ 
public 

class 
PermissionService "
:# $
IPermissionService% 7
{ 
private 
readonly !
IPermissionRepository .!
_permissionRepository/ D
;D E
private 
readonly 
ILoggerManager '
<' (
PermissionService( 9
>9 :
_logger; B
;B C
private 
readonly 
IUnitOfWork $
_unitOfWork% 0
;0 1
private 
readonly 
IMapper  
_mapper! (
;( )
public'' 
PermissionService''  
(''  !!
IPermissionRepository(( ! 
permissionRepository((" 6
,((6 7
ILoggerManager)) 
<)) 
PermissionService)) ,
>)), -
logger)). 4
,))4 5
IUnitOfWork** 

unitOfWork** "
,**" #
IMapper++ 
mapper++ 
)++ 
{,, 	!
_permissionRepository-- !
=--" # 
permissionRepository--$ 8
;--8 9
_logger.. 
=.. 
logger.. 
;.. 
_unitOfWork// 
=// 

unitOfWork// $
;//$ %
_mapper00 
=00 
mapper00 
;00 
}11 	
public77 
async77 
Task77 
<77 
List77 
<77 
PermissionDto77 ,
>77, -
>77- ."
GetAllPermissionsAsync77/ E
(77E F
)77F G
{88 	
try99 
{:: 
var;; 
spec;; 
=;; 
new;; 
AllSpecification;; /
<;;/ 0

Permission;;0 :
>;;: ;
(;;; <
);;< =
;;;= >
var<< 
permissions<< 
=<<  !
await<<" '!
_permissionRepository<<( =
.<<= >
	ListAsync<<> G
(<<G H
spec<<H L
)<<L M
;<<M N
return== 
permissions== "
.==" #
Select==# )
(==) *
p==* +
=>==, .
MapToDto==/ 7
(==7 8
p==8 9
)==9 :
)==: ;
.==; <
ToList==< B
(==B C
)==C D
;==D E
}>> 
catch?? 
(?? 
	Exception?? 
ex?? 
)??  
{@@ 
_loggerAA 
.AA 
LogErrorAA  
(AA  !
exAA! #
,AA# $
LogMessagesAA% 0
.AA0 1&
ErrorGettingAllPermissionsAA1 K
)AAK L
;AAL M
returnBB 
newBB 
ListBB 
<BB  
PermissionDtoBB  -
>BB- .
(BB. /
)BB/ 0
;BB0 1
}CC 
}DD 	
publicJJ 
asyncJJ 
TaskJJ 
<JJ 
ListJJ 
<JJ 
PermissionGroupDtoJJ 1
>JJ1 2
>JJ2 3'
GetPermissionsByGroupsAsyncJJ4 O
(JJO P
)JJP Q
{KK 	
tryLL 
{MM 
varNN 
specNN 
=NN 
newNN 
AllSpecificationNN /
<NN/ 0

PermissionNN0 :
>NN: ;
(NN; <
)NN< =
;NN= >
varOO 
permissionsOO 
=OO  !
awaitOO" '!
_permissionRepositoryOO( =
.OO= >
	ListAsyncOO> G
(OOG H
specOOH L
)OOL M
;OOM N
varQQ 
groupedPermissionsQQ &
=QQ' (
permissionsQQ) 4
.RR 
GroupByRR 
(RR 
pRR 
=>RR !
pRR" #
.RR# $
GroupRR$ )
??RR* , 
PermissionGroupNamesRR- A
.RRA B
OtherRRB G
)RRG H
.SS 
SelectSS 
(SS 
gSS 
=>SS  
newSS! $
PermissionGroupDtoSS% 7
{TT 
GroupUU 
=UU 
gUU  !
.UU! "
KeyUU" %
,UU% &
PermissionsVV #
=VV$ %
gVV& '
.VV' (
SelectVV( .
(VV. /
pVV/ 0
=>VV1 3
MapToDtoVV4 <
(VV< =
pVV= >
)VV> ?
)VV? @
.VV@ A
OrderByVVA H
(VVH I
pVVI J
=>VVK M
pVVN O
.VVO P
NameVVP T
)VVT U
.VVU V
ToListVVV \
(VV\ ]
)VV] ^
}WW 
)WW 
.XX 
OrderByXX 
(XX 
gXX 
=>XX !
gXX" #
.XX# $
GroupXX$ )
)XX) *
.YY 
ToListYY 
(YY 
)YY 
;YY 
return[[ 
groupedPermissions[[ )
;[[) *
}\\ 
catch]] 
(]] 
	Exception]] 
ex]] 
)]]  
{^^ 
_logger__ 
.__ 
LogError__  
(__  !
ex__! #
,__# $
LogMessages__% 0
.__0 1*
ErrorGettingPermissionsByGroup__1 O
)__O P
;__P Q
return`` 
new`` 
List`` 
<``  
PermissionGroupDto``  2
>``2 3
(``3 4
)``4 5
;``5 6
}aa 
}bb 	
publicii 
asyncii 
Taskii 
<ii 
Listii 
<ii 
PermissionDtoii ,
>ii, -
>ii- .'
GetPermissionsByRoleIdAsyncii/ J
(iiJ K
intiiK N
roleIdiiO U
)iiU V
{jj 	
trykk 
{ll 
varmm 
rolePermissionRepomm &
=mm' (
_unitOfWorkmm) 4
.mm4 5
GetRepositorymm5 B
<mmB C
RolePermissionmmC Q
>mmQ R
(mmR S
)mmS T
;mmT U
varnn 
specnn 
=nn 
newnn ,
 PermissionsByRoleIdSpecificationnn ?
(nn? @
roleIdnn@ F
)nnF G
;nnG H
varoo 
rolePermissionsoo #
=oo$ %
awaitoo& +
rolePermissionRepooo, >
.oo> ?
	ListAsyncoo? H
(ooH I
specooI M
)ooM N
;ooN O
returnqq 
rolePermissionsqq &
.rr 
Whererr 
(rr 
rprr 
=>rr  
rprr! #
.rr# $

Permissionrr$ .
!=rr/ 1
nullrr2 6
)rr6 7
.ss 
Selectss 
(ss 
rpss 
=>ss !
MapToDtoss" *
(ss* +
rpss+ -
.ss- .

Permissionss. 8
)ss8 9
)ss9 :
.tt 
ToListtt 
(tt 
)tt 
;tt 
}uu 
catchvv 
(vv 
	Exceptionvv 
exvv 
)vv  
{ww 
_loggerxx 
.xx 
LogErrorxx  
(xx  !
exxx! #
,xx# $
LogMessagesxx% 0
.xx0 1)
ErrorGettingPermissionsByRolexx1 N
,xxN O
roleIdxxP V
)xxV W
;xxW X
returnyy 
newyy 
Listyy 
<yy  
PermissionDtoyy  -
>yy- .
(yy. /
)yy/ 0
;yy0 1
}zz 
}{{ 	
public
�� 
async
�� 
Task
�� 
<
�� 
List
�� 
<
�� 
PermissionDto
�� ,
>
��, -
>
��- .)
GetPermissionsByUserIdAsync
��/ J
(
��J K
int
��K N
userId
��O U
)
��U V
{
�� 	
try
�� 
{
�� 
var
�� 
userRepo
�� 
=
�� 
_unitOfWork
�� *
.
��* +
Users
��+ 0
;
��0 1
var
��  
userPermissionRepo
�� &
=
��' (
_unitOfWork
��) 4
.
��4 5
GetRepository
��5 B
<
��B C
UserPermission
��C Q
>
��Q R
(
��R S
)
��S T
;
��T U
var
�� 
userSpec
�� 
=
�� 
new
�� "5
'UserWithRoleAndPermissionsSpecification
��# J
(
��J K
userId
��K Q
)
��Q R
;
��R S
var
�� 
user
�� 
=
�� 
await
��  
userRepo
��! )
.
��) *!
FirstOrDefaultAsync
��* =
(
��= >
userSpec
��> F
)
��F G
;
��G H
if
�� 
(
�� 
user
�� 
==
�� 
null
��  
)
��  !
{
�� 
_logger
�� 
.
�� 
LogWarn
�� #
(
��# $
LogMessages
��$ /
.
��/ 0
UserNotFoundById
��0 @
,
��@ A
userId
��B H
)
��H I
;
��I J
return
�� 
new
�� 
List
�� #
<
��# $
PermissionDto
��$ 1
>
��1 2
(
��2 3
)
��3 4
;
��4 5
}
�� 
var
�� 
allPermissions
�� "
=
��# $
new
��% (

Dictionary
��) 3
<
��3 4
int
��4 7
,
��7 8
PermissionDto
��9 F
>
��F G
(
��G H
)
��H I
;
��I J
if
�� 
(
�� 
user
�� 
.
�� 
Role
�� 
?
�� 
.
�� 
RolePermissions
�� .
!=
��/ 1
null
��2 6
)
��6 7
{
�� 
foreach
�� 
(
�� 
var
��  
rp
��! #
in
��$ &
user
��' +
.
��+ ,
Role
��, 0
.
��0 1
RolePermissions
��1 @
)
��@ A
{
�� 
if
�� 
(
�� 
rp
�� 
.
�� 

Permission
�� )
!=
��* ,
null
��- 1
&&
��2 4
!
��5 6
allPermissions
��6 D
.
��D E
ContainsKey
��E P
(
��P Q
rp
��Q S
.
��S T

Permission
��T ^
.
��^ _
Id
��_ a
)
��a b
)
��b c
{
�� 
allPermissions
�� *
.
��* +
Add
��+ .
(
��. /
rp
��/ 1
.
��1 2

Permission
��2 <
.
��< =
Id
��= ?
,
��? @
MapToDto
��A I
(
��I J
rp
��J L
.
��L M

Permission
��M W
,
��W X
true
��Y ]
,
��] ^
user
��_ c
.
��c d
Role
��d h
.
��h i
Name
��i m
)
��m n
)
��n o
;
��o p
}
�� 
}
�� 
}
�� 
var
�� !
userPermissionsSpec
�� '
=
��( )
new
��* -5
'UserPermissionsWithDetailsSpecification
��. U
(
��U V
userId
��V \
)
��\ ]
;
��] ^
var
�� 
userPermissions
�� #
=
��$ %
await
��& + 
userPermissionRepo
��, >
.
��> ?
	ListAsync
��? H
(
��H I!
userPermissionsSpec
��I \
)
��\ ]
;
��] ^
foreach
�� 
(
�� 
var
�� 
up
�� 
in
��  "
userPermissions
��# 2
)
��2 3
{
�� 
if
�� 
(
�� 
up
�� 
.
�� 

Permission
�� %
!=
��& (
null
��) -
)
��- .
{
�� 
if
�� 
(
�� 
allPermissions
�� *
.
��* +
TryGetValue
��+ 6
(
��6 7
up
��7 9
.
��9 :

Permission
��: D
.
��D E
Id
��E G
,
��G H
out
��I L
var
��M P
existingDto
��Q \
)
��\ ]
)
��] ^
{
�� 
if
�� 
(
��  
!
��  !
up
��! #
.
��# $
	IsGranted
��$ -
)
��- .
{
�� 
existingDto
��  +
.
��+ ,
	IsGranted
��, 5
=
��6 7
false
��8 =
;
��= >
existingDto
��  +
.
��+ ,
IsCustom
��, 4
=
��5 6
true
��7 ;
;
��; <
}
�� 
else
��  
if
��! #
(
��$ %
existingDto
��% 0
.
��0 1

IsFromRole
��1 ;
)
��; <
{
�� 
existingDto
��  +
.
��+ ,
IsCustom
��, 4
=
��5 6
true
��7 ;
;
��; <
}
�� 
}
�� 
else
�� 
if
�� 
(
��  !
up
��! #
.
��# $
	IsGranted
��$ -
)
��- .
{
�� 
var
�� 
newDto
��  &
=
��' (
MapToDto
��) 1
(
��1 2
up
��2 4
.
��4 5

Permission
��5 ?
,
��? @
false
��A F
)
��F G
;
��G H
newDto
�� "
.
��" #
IsCustom
��# +
=
��, -
true
��. 2
;
��2 3
allPermissions
�� *
.
��* +
Add
��+ .
(
��. /
up
��/ 1
.
��1 2

Permission
��2 <
.
��< =
Id
��= ?
,
��? @
newDto
��A G
)
��G H
;
��H I
}
�� 
}
�� 
}
�� 
return
�� 
allPermissions
�� %
.
��% &
Values
��& ,
.
��, -
OrderBy
��- 4
(
��4 5
p
��5 6
=>
��7 9
p
��: ;
.
��; <
Group
��< A
)
��A B
.
��B C
ThenBy
��C I
(
��I J
p
��J K
=>
��L N
p
��O P
.
��P Q
Name
��Q U
)
��U V
.
��V W
ToList
��W ]
(
��] ^
)
��^ _
;
��_ `
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
_logger
�� 
.
�� 
LogError
��  
(
��  !
ex
��! #
,
��# $
LogMessages
��% 0
.
��0 1+
ErrorGettingPermissionsByUser
��1 N
,
��N O
userId
��P V
)
��V W
;
��W X
return
�� 
new
�� 
List
�� 
<
��  
PermissionDto
��  -
>
��- .
(
��. /
)
��/ 0
;
��0 1
}
�� 
}
�� 	
public
�� 
async
�� 
Task
�� 
<
�� 
bool
�� 
>
�� *
AssignPermissionsToRoleAsync
��  <
(
��< =
int
��= @
roleId
��A G
,
��G H
List
��I M
<
��M N
int
��N Q
>
��Q R
permissionIds
��S `
)
��` a
{
�� 	
try
�� 
{
�� 
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,(
AssigningPermissionsToRole
��, F
,
��F G
roleId
��H N
,
��N O
permissionIds
��P ]
?
��] ^
.
��^ _
Count
��_ d
??
��e g
$num
��h i
)
��i j
;
��j k
var
��  
rolePermissionRepo
�� &
=
��' (
_unitOfWork
��) 4
.
��4 5
GetRepository
��5 B
<
��B C
RolePermission
��C Q
>
��Q R
(
��R S
)
��S T
;
��T U
var
�� %
existingPermissionsSpec
�� +
=
��, -
new
��. 12
$RolePermissionsByRoleIdSpecification
��2 V
(
��V W
roleId
��W ]
)
��] ^
;
��^ _
var
�� !
existingPermissions
�� '
=
��( )
await
��* / 
rolePermissionRepo
��0 B
.
��B C
	ListAsync
��C L
(
��L M%
existingPermissionsSpec
��M d
)
��d e
;
��e f
if
�� 
(
�� !
existingPermissions
�� '
.
��' (
Any
��( +
(
��+ ,
)
��, -
)
��- .
{
��  
rolePermissionRepo
�� &
.
��& '
DeleteRange
��' 2
(
��2 3!
existingPermissions
��3 F
)
��F G
;
��G H
_logger
�� 
.
�� 
LogDebug
�� $
(
��$ %
$str
��% t
,
��t u"
existingPermissions��v �
.��� �
Count��� �
,��� �
roleId��� �
)��� �
;��� �
}
�� 
if
�� 
(
�� 
permissionIds
�� !
!=
��" $
null
��% )
&&
��* ,
permissionIds
��- :
.
��: ;
Count
��; @
>
��A B
$num
��C D
)
��D E
{
�� 
var
�� 
newPermissions
�� &
=
��' (
permissionIds
��) 6
.
��6 7
Select
��7 =
(
��= >
pid
��> A
=>
��B D
new
��E H
RolePermission
��I W
{
�� 
RoleId
�� 
=
��  
roleId
��! '
,
��' (
PermissionId
�� $
=
��% &
pid
��' *
}
�� 
)
�� 
.
�� 
ToList
�� 
(
�� 
)
�� 
;
��  
await
��  
rolePermissionRepo
�� ,
.
��, -
AddRangeAsync
��- :
(
��: ;
newPermissions
��; I
)
��I J
;
��J K
_logger
�� 
.
�� 
LogDebug
�� $
(
��$ %
$str
��% a
,
��a b
newPermissions
��c q
.
��q r
Count
��r w
,
��w x
roleId
��y 
)�� �
;��� �
}
�� 
await
�� 
_unitOfWork
�� !
.
��! "
SaveChangesAsync
��" 2
(
��2 3
)
��3 4
;
��4 5
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,'
PermissionsAssignedToRole
��, E
,
��E F
roleId
��G M
,
��M N
permissionIds
��O \
?
��\ ]
.
��] ^
Count
��^ c
??
��d f
$num
��g h
)
��h i
;
��i j
return
�� 
true
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
_logger
�� 
.
�� 
LogError
��  
(
��  !
ex
��! #
,
��# $
LogMessages
��% 0
.
��0 1-
ErrorAssigningPermissionsToRole
��1 P
,
��P Q
roleId
��R X
)
��X Y
;
��Y Z
return
�� 
false
�� 
;
�� 
}
�� 
}
�� 	
public
�� 
async
�� 
Task
�� 
<
�� 
bool
�� 
>
�� )
AssignPermissionToUserAsync
��  ;
(
��; <
int
��< ?
userId
��@ F
,
��F G
int
��H K
permissionId
��L X
,
��X Y
bool
��Z ^
	isGranted
��_ h
)
��h i
{
�� 	
try
�� 
{
�� 
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,'
AssigningPermissionToUser
��, E
,
��E F
userId
��G M
,
��M N
permissionId
��O [
,
��[ \
	isGranted
��] f
)
��f g
;
��g h
var
��  
userPermissionRepo
�� &
=
��' (
_unitOfWork
��) 4
.
��4 5
GetRepository
��5 B
<
��B C
UserPermission
��C Q
>
��Q R
(
��R S
)
��S T
;
��T U
var
�� 
spec
�� 
=
�� 
new
�� )
UserPermissionSpecification
�� :
(
��: ;
userId
��; A
,
��A B
permissionId
��C O
)
��O P
;
��P Q
var
��  
existingPermission
�� &
=
��' (
await
��) . 
userPermissionRepo
��/ A
.
��A B!
FirstOrDefaultAsync
��B U
(
��U V
spec
��V Z
)
��Z [
;
��[ \
if
�� 
(
��  
existingPermission
�� &
!=
��' )
null
��* .
)
��. /
{
�� 
if
�� 
(
��  
existingPermission
�� *
.
��* +
	IsGranted
��+ 4
!=
��5 7
	isGranted
��8 A
)
��A B
{
��  
existingPermission
�� *
.
��* +
	IsGranted
��+ 4
=
��5 6
	isGranted
��7 @
;
��@ A 
userPermissionRepo
�� *
.
��* +
Update
��+ 1
(
��1 2 
existingPermission
��2 D
)
��D E
;
��E F
_logger
�� 
.
��  
LogInfo
��  '
(
��' (
LogMessages
��( 3
.
��3 4#
UserPermissionUpdated
��4 I
,
��I J
	isGranted
��K T
,
��T U
userId
��V \
,
��\ ]
permissionId
��^ j
)
��j k
;
��k l
}
�� 
}
�� 
else
�� 
{
�� 
var
�� 
newUserPermission
�� )
=
��* +
new
��, /
UserPermission
��0 >
{
�� 
UserId
�� 
=
��  
userId
��! '
,
��' (
PermissionId
�� $
=
��% &
permissionId
��' 3
,
��3 4
	IsGranted
�� !
=
��" #
	isGranted
��$ -
}
�� 
;
�� 
await
��  
userPermissionRepo
�� ,
.
��, -
AddAsync
��- 5
(
��5 6
newUserPermission
��6 G
)
��G H
;
��H I
_logger
�� 
.
�� 
LogInfo
�� #
(
��# $
LogMessages
��$ /
.
��/ 0!
UserPermissionAdded
��0 C
,
��C D
	isGranted
��E N
,
��N O
userId
��P V
,
��V W
permissionId
��X d
)
��d e
;
��e f
}
�� 
await
�� 
_unitOfWork
�� !
.
��! "
SaveChangesAsync
��" 2
(
��2 3
)
��3 4
;
��4 5
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,&
PermissionAssignedToUser
��, D
,
��D E
userId
��F L
,
��L M
permissionId
��N Z
,
��Z [
	isGranted
��\ e
)
��e f
;
��f g
return
�� 
true
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
_logger
�� 
.
�� 
LogError
��  
(
��  !
ex
��! #
,
��# $
LogMessages
��% 0
.
��0 1*
ErrorAssigningUserPermission
��1 M
,
��M N
userId
��O U
,
��U V
permissionId
��W c
)
��c d
;
��d e
return
�� 
false
�� 
;
�� 
}
�� 
}
�� 	
public
�� 
async
�� 
Task
�� 
<
�� 
bool
�� 
>
�� '
RemoveUserPermissionAsync
��  9
(
��9 :
int
��: =
userId
��> D
,
��D E
int
��F I
permissionId
��J V
)
��V W
{
�� 	
try
�� 
{
�� 
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,(
RemovingPermissionFromUser
��, F
,
��F G
userId
��H N
,
��N O
permissionId
��P \
)
��\ ]
;
��] ^
var
��  
userPermissionRepo
�� &
=
��' (
_unitOfWork
��) 4
.
��4 5
GetRepository
��5 B
<
��B C
UserPermission
��C Q
>
��Q R
(
��R S
)
��S T
;
��T U
var
�� 
spec
�� 
=
�� 
new
�� )
UserPermissionSpecification
�� :
(
��: ;
userId
��; A
,
��A B
permissionId
��C O
)
��O P
;
��P Q
var
�� 
userPermission
�� "
=
��# $
await
��% * 
userPermissionRepo
��+ =
.
��= >!
FirstOrDefaultAsync
��> Q
(
��Q R
spec
��R V
)
��V W
;
��W X
if
�� 
(
�� 
userPermission
�� "
!=
��# %
null
��& *
)
��* +
{
��  
userPermissionRepo
�� &
.
��& '
Delete
��' -
(
��- .
userPermission
��. <
)
��< =
;
��= >
await
�� 
_unitOfWork
�� %
.
��% &
SaveChangesAsync
��& 6
(
��6 7
)
��7 8
;
��8 9
_logger
�� 
.
�� 
LogInfo
�� #
(
��# $
LogMessages
��$ /
.
��/ 0'
PermissionRemovedFromUser
��0 I
,
��I J
userId
��K Q
,
��Q R
permissionId
��S _
)
��_ `
;
��` a
return
�� 
true
�� 
;
��  
}
�� 
_logger
�� 
.
�� 
LogWarn
�� 
(
��  
LogMessages
��  +
.
��+ ,.
 UserPermissionNotFoundForRemoval
��, L
,
��L M
userId
��N T
,
��T U
permissionId
��V b
)
��b c
;
��c d
return
�� 
true
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
_logger
�� 
.
�� 
LogError
��  
(
��  !
ex
��! #
,
��# $
LogMessages
��% 0
.
��0 1)
ErrorRemovingUserPermission
��1 L
,
��L M
userId
��N T
,
��T U
permissionId
��V b
)
��b c
;
��c d
return
�� 
false
�� 
;
�� 
}
�� 
}
�� 	
public
�� 
async
�� 
Task
�� 
<
�� 
bool
�� 
>
�� -
ResetUserToRolePermissionsAsync
��  ?
(
��? @
int
��@ C
userId
��D J
)
��J K
{
�� 	
try
�� 
{
�� 
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,)
ResettingPermissionsForUser
��, G
,
��G H
userId
��I O
)
��O P
;
��P Q
var
��  
userPermissionRepo
�� &
=
��' (
_unitOfWork
��) 4
.
��4 5
GetRepository
��5 B
<
��B C
UserPermission
��C Q
>
��Q R
(
��R S
)
��S T
;
��T U
var
�� 
spec
�� 
=
�� 
new
�� 2
$UserPermissionsByUserIdSpecification
�� C
(
��C D
userId
��D J
)
��J K
;
��K L
var
�� 
userPermissions
�� #
=
��$ %
await
��& + 
userPermissionRepo
��, >
.
��> ?
	ListAsync
��? H
(
��H I
spec
��I M
)
��M N
;
��N O
if
�� 
(
�� 
userPermissions
�� #
.
��# $
Any
��$ '
(
��' (
)
��( )
)
��) *
{
��  
userPermissionRepo
�� &
.
��& '
DeleteRange
��' 2
(
��2 3
userPermissions
��3 B
)
��B C
;
��C D
await
�� 
_unitOfWork
�� %
.
��% &
SaveChangesAsync
��& 6
(
��6 7
)
��7 8
;
��8 9
_logger
�� 
.
�� 
LogInfo
�� #
(
��# $
LogMessages
��$ /
.
��/ 0%
PermissionsResetForUser
��0 G
,
��G H
userId
��I O
)
��O P
;
��P Q
return
�� 
true
�� 
;
��  
}
�� 
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,*
UserPermissionsResetNoAction
��, H
,
��H I
userId
��J P
)
��P Q
;
��Q R
return
�� 
true
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
_logger
�� 
.
�� 
LogError
��  
(
��  !
ex
��! #
,
��# $
LogMessages
��% 0
.
��0 1+
ErrorResettingUserPermissions
��1 N
,
��N O
userId
��P V
)
��V W
;
��W X
return
�� 
false
�� 
;
�� 
}
�� 
}
�� 	
public
�� 
async
�� 
Task
�� 
<
�� 
bool
�� 
>
�� $
UserHasPermissionAsync
��  6
(
��6 7
int
��7 :
userId
��; A
,
��A B
string
��C I
permissionName
��J X
)
��X Y
{
�� 	
try
�� 
{
�� 
var
�� 
userRepo
�� 
=
�� 
_unitOfWork
�� *
.
��* +
Users
��+ 0
;
��0 1
var
��  
userPermissionRepo
�� &
=
��' (
_unitOfWork
��) 4
.
��4 5
GetRepository
��5 B
<
��B C
UserPermission
��C Q
>
��Q R
(
��R S
)
��S T
;
��T U
var
�� 
userSpec
�� 
=
�� 
new
�� "%
EntityByIdSpecification
��# :
<
��: ;
User
��; ?
>
��? @
(
��@ A
userId
��A G
,
��G H
u
��I J
=>
��K M
u
��N O
.
��O P
Role
��P T
)
��T U
;
��U V
var
�� 
user
�� 
=
�� 
await
��  
userRepo
��! )
.
��) *!
FirstOrDefaultAsync
��* =
(
��= >
userSpec
��> F
)
��F G
;
��G H
if
�� 
(
�� 
user
�� 
==
�� 
null
��  
)
��  !
{
�� 
_logger
�� 
.
�� 
LogDebug
�� $
(
��$ %
$str
��% `
,
��` a
userId
��b h
)
��h i
;
��i j
return
�� 
false
��  
;
��  !
}
�� 
var
�� #
hasPermissionFromRole
�� )
=
��* +
user
��, 0
.
��0 1
Role
��1 5
!=
��6 8
null
��9 =
&&
��> @
user
��A E
.
��E F
Role
��F J
.
��J K
RolePermissions
��K Z
!=
��[ ]
null
��^ b
&&
��c e
user
��* .
.
��. /
Role
��/ 3
.
��3 4
RolePermissions
��4 C
.
��C D
Any
��D G
(
��G H
rp
��H J
=>
��K M
rp
��N P
.
��P Q

Permission
��Q [
?
��[ \
.
��\ ]
Name
��] a
==
��b d
permissionName
��e s
)
��s t
;
��t u
if
�� 
(
�� #
hasPermissionFromRole
�� )
)
��) *
{
�� 
_logger
�� 
.
�� 
LogDebug
�� $
(
��$ %
$str
��% j
,
��j k
userId
��l r
,
��r s
permissionName��t �
)��� �
;��� �
return
�� 
true
�� 
;
��  
}
�� 
_logger
�� 
.
�� 
LogDebug
��  
(
��  !
$str
��! x
,
��x y
userId��z �
,��� �
permissionName��� �
)��� �
;��� �
return
�� 
false
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
_logger
�� 
.
�� 
LogError
��  
(
��  !
ex
��! #
,
��# $
LogMessages
��% 0
.
��0 1)
ErrorCheckingUserPermission
��1 L
,
��L M
userId
��N T
,
��T U
permissionName
��V d
)
��d e
;
��e f
return
�� 
false
�� 
;
�� 
}
�� 
}
�� 	
private
�� 
PermissionDto
�� 
MapToDto
�� &
(
��& '

Permission
��' 1

permission
��2 <
,
��< =
bool
��> B

isFromRole
��C M
=
��N O
false
��P U
,
��U V
string
��W ]
?
��] ^
roleName
��_ g
=
��h i
null
��j n
)
��n o
{
�� 	
return
�� 
new
�� 
PermissionDto
�� $
{
�� 
Id
�� 
=
�� 

permission
�� 
.
��  
Id
��  "
,
��" #
Name
�� 
=
�� 

permission
�� !
.
��! "
Name
��" &
,
��& '
Description
�� 
=
�� 

permission
�� (
.
��( )
Description
��) 4
,
��4 5
ResourceType
�� 
=
�� 

permission
�� )
.
��) *
ResourceType
��* 6
??
��7 9
$str
��: <
,
��< =
Group
�� 
=
�� 

permission
�� "
.
��" #
Group
��# (
,
��( )

IsFromRole
�� 
=
�� 

isFromRole
�� '
,
��' (
IsCustom
�� 
=
�� 
!
�� 

isFromRole
�� &
,
��& '
RoleName
�� 
=
�� 

isFromRole
�� %
?
��& '
roleName
��( 0
:
��1 2
null
��3 7
,
��7 8
	IsGranted
�� 
=
�� 
true
��  
}
�� 
;
�� 
}
�� 	
}
�� 
}�� �L
\C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Services\PasswordHasher.cs
	namespace		 	
Stock		
 
.		 
Infrastructure		 
.		 
Services		 '
{

 
public 

class 
PasswordHasher 
:  !
IPasswordHasher" 1
{ 
private 
const 
int 
SaltSize "
=# $
$num% '
;' (
private 
const 
int 
KeySize !
=" #
$num$ &
;& '
private 
const 
int 

Iterations $
=% &
$num' ,
;, -
private 
static 
readonly 
HashAlgorithmName  1
_hashAlgorithmName2 D
=E F
HashAlgorithmNameG X
.X Y
SHA256Y _
;_ `
private 
const 
char 
	Delimiter $
=% &
$char' *
;* +
private 
const 
int 
WORK_FACTOR %
=& '
$num( *
;* +
private 
readonly 
ILoggerManager '
<' (
PasswordHasher( 6
>6 7
_logger8 ?
;? @
public 
PasswordHasher 
( 
ILoggerManager ,
<, -
PasswordHasher- ;
>; <
logger= C
)C D
{ 	
_logger 
= 
logger 
; 
} 	
public 
string 
HashPassword "
(" #
string# )
password* 2
)2 3
{ 	
if 
( 
string 
. 
IsNullOrEmpty $
($ %
password% -
)- .
). /
{ 
_logger 
. 
LogError  
(  !
LogMessages! ,
., -1
%PasswordCannotBeNullOrEmptyForHashing- R
)R S
;S T
throw   
new   !
ArgumentNullException   /
(  / 0
nameof  0 6
(  6 7
password  7 ?
)  ? @
,  @ A
ErrorMessages  B O
.  O P'
PasswordCannotBeNullOrEmpty  P k
)  k l
;  l m
}!! 
try## 
{$$ 
_logger%% 
.%% 
LogInfo%% 
(%%  
LogMessages%%  +
.%%+ ,"
PasswordHashingStarted%%, B
,%%B C
WORK_FACTOR%%D O
)%%O P
;%%P Q
var'' 
hashedPassword'' "
=''# $
BCrypt''% +
.''+ ,
Net'', /
.''/ 0
BCrypt''0 6
.''6 7
HashPassword''7 C
(''C D
password''D L
,''L M
WORK_FACTOR''N Y
)''Y Z
;''Z [
_logger)) 
.)) 
LogInfo)) 
())  
LogMessages))  +
.))+ ,&
PasswordHashedSuccessfully)), F
,))F G
hashedPassword))H V
)))V W
;))W X
LogHashDetails** 
(** 
hashedPassword** -
)**- .
;**. /
return,, 
hashedPassword,, %
;,,% &
}-- 
catch.. 
(.. 
BCrypt.. 
... 
Net.. 
... 
SaltParseException.. 0
ex..1 3
)..3 4
{// 
_logger00 
.00 
LogError00  
(00  !
ex00! #
,00# $
LogMessages00% 0
.000 1$
PasswordHashingSaltError001 I
)00I J
;00J K
throw11 
new11 %
InvalidOperationException11 3
(113 4
ErrorMessages114 A
.11A B!
PasswordHashingFailed11B W
,11W X
ex11Y [
)11[ \
;11\ ]
}22 
catch33 
(33 
	Exception33 
ex33 
)33  
{44 
_logger55 
.55 
LogError55  
(55  !
ex55! #
,55# $
LogMessages55% 0
.550 1'
PasswordHashingGenericError551 L
)55L M
;55M N
throw66 
new66 %
InvalidOperationException66 3
(663 4
ErrorMessages664 A
.66A B!
PasswordHashingFailed66B W
,66W X
ex66Y [
)66[ \
;66\ ]
}77 
}88 	
public:: 
bool:: 
VerifyPassword:: "
(::" #
string::# )
password::* 2
,::2 3
string::4 :
passwordHash::; G
)::G H
{;; 	
if<< 
(<< 
string<< 
.<< 
IsNullOrEmpty<< $
(<<$ %
password<<% -
)<<- .
||<</ 1
string<<2 8
.<<8 9
IsNullOrEmpty<<9 F
(<<F G
passwordHash<<G S
)<<S T
)<<T U
{== 
_logger>> 
.>> 
LogError>>  
(>>  !
LogMessages>>! ,
.>>, -<
0PasswordOrHashCannotBeNullOrEmptyForVerification>>- ]
)>>] ^
;>>^ _
return@@ 
false@@ 
;@@ 
}AA 
tryCC 
{DD 
_loggerEE 
.EE 
LogInfoEE 
(EE  
LogMessagesEE  +
.EE+ ,'
PasswordVerificationStartedEE, G
)EEG H
;EEH I
LogHashDetailsFF 
(FF 
passwordHashFF +
)FF+ ,
;FF, -
varHH 
resultHH 
=HH 
BCryptHH #
.HH# $
NetHH$ '
.HH' (
BCryptHH( .
.HH. /
VerifyHH/ 5
(HH5 6
passwordHH6 >
,HH> ?
passwordHashHH@ L
)HHL M
;HHM N
_loggerJJ 
.JJ 
LogInfoJJ 
(JJ  
LogMessagesJJ  +
.JJ+ ,)
PasswordVerificationResultLogJJ, I
,JJI J
resultJJK Q
)JJQ R
;JJR S
returnKK 
resultKK 
;KK 
}LL 
catchMM 
(MM 
BCryptMM 
.MM 
NetMM 
.MM 
SaltParseExceptionMM 0
exMM1 3
)MM3 4
{NN 
_loggerOO 
.OO 
LogErrorOO  
(OO  !
exOO! #
,OO# $
LogMessagesOO% 0
.OO0 1)
PasswordVerificationSaltErrorOO1 N
)OON O
;OOO P
returnQQ 
falseQQ 
;QQ 
}RR 
catchSS 
(SS 
	ExceptionSS 
exSS 
)SS  
{TT 
_loggerUU 
.UU 
LogErrorUU  
(UU  !
exUU! #
,UU# $
LogMessagesUU% 0
.UU0 1,
 PasswordVerificationGenericErrorUU1 Q
)UUQ R
;UUR S
returnWW 
falseWW 
;WW 
}XX 
}YY 	
private\\ 
void\\ 
LogHashDetails\\ #
(\\# $
string\\$ *
hash\\+ /
)\\/ 0
{]] 	
if^^ 
(^^ 
string^^ 
.^^ 
IsNullOrEmpty^^ $
(^^$ %
hash^^% )
)^^) *
)^^* +
return^^, 2
;^^2 3
_logger`` 
.`` 
LogDebug`` 
(`` 
LogMessages`` (
.``( )
HashFormatLog``) 6
,``6 7
(``8 9
hash``9 =
.``= >

StartsWith``> H
(``H I
$str``I M
)``M N
?``O P
HashFormats``Q \
.``\ ]
BCrypt``] c
:``d e
HashFormats``f q
.``q r
Unknown``r y
)``y z
)``z {
;``{ |
_loggeraa 
.aa 
LogDebugaa 
(aa 
LogMessagesaa (
.aa( )
HashLengthLogaa) 6
,aa6 7
hashaa8 <
.aa< =
Lengthaa= C
)aaC D
;aaD E
trybb 
{cc 
varee 
partsee 
=ee 
hashee  
.ee  !
Splitee! &
(ee& '
$charee' *
)ee* +
;ee+ ,
ifff 
(ff 
partsff 
.ff 
Lengthff  
>ff! "
$numff# $
&&ff% '
intff( +
.ff+ ,
TryParseff, 4
(ff4 5
partsff5 :
[ff: ;
$numff; <
]ff< =
,ff= >
outff? B
intffC F

workFactorffG Q
)ffQ R
)ffR S
{gg 
_loggerhh 
.hh 
LogDebughh $
(hh$ %
LogMessageshh% 0
.hh0 1
WorkFactorLoghh1 >
,hh> ?

workFactorhh@ J
)hhJ K
;hhK L
}ii 
}jj 
catchkk 
(kk 
	Exceptionkk 
exkk 
)kk  
{ll 
_loggermm 
.mm 
LogErrormm  
(mm  !
exmm! #
,mm# $
LogMessagesmm% 0
.mm0 1"
ErrorParsingWorkFactormm1 G
)mmG H
;mmH I
}nn 
}oo 	
}pp 
internalss 
staticss 
classss 
HashFormatsss %
{tt 
publicuu 
constuu 
stringuu 
BCryptuu "
=uu# $
$struu% -
;uu- .
publicvv 
constvv 
stringvv 
Unknownvv #
=vv$ %
$strvv& 2
;vv2 3
}ww 
}xx �:
_C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Services\JwtTokenGenerator.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Services '
{ 
public 

class 
JwtTokenGenerator "
:# $
IJwtTokenGenerator% 7
{ 
private 
readonly 
IConfiguration '
_configuration( 6
;6 7
private 
readonly 
IPermissionService +
_permissionService, >
;> ?
public 
JwtTokenGenerator  
(  !
IConfiguration! /
configuration0 =
,= >
IPermissionService? Q
permissionServiceR c
)c d
{ 	
_configuration 
= 
configuration *
;* +
_permissionService 
=  
permissionService! 2
;2 3
} 	
public 
async 
Task 
< 
string  
>  !
GenerateTokenAsync" 4
(4 5
User5 9
user: >
)> ?
{ 	
var 
tokenHandler 
= 
new "#
JwtSecurityTokenHandler# :
(: ;
); <
;< =
var 
jwtKey 
= 
_configuration '
[' (
$str( 1
]1 2
;2 3
if 
( 
string 
. 
IsNullOrEmpty $
($ %
jwtKey% +
)+ ,
), -
{   
throw!! 
new!! %
InvalidOperationException!! 3
(!!3 4
$str!!4 _
)!!_ `
;!!` a
}"" 
var## 
key## 
=## 
Encoding## 
.## 
ASCII## $
.##$ %
GetBytes##% -
(##- .
jwtKey##. 4
)##4 5
;##5 6
var%% 
claims%% 
=%% 
new%% 
List%% !
<%%! "
Claim%%" '
>%%' (
{&& 
new'' 
Claim'' 
('' 

ClaimTypes'' $
.''$ %
NameIdentifier''% 3
,''3 4
user''5 9
.''9 :
Id'': <
.''< =
ToString''= E
(''E F
)''F G
)''G H
,''H I
new(( 
Claim(( 
((( 

ClaimTypes(( $
.(($ %
Name((% )
,(() *
$"((+ -
{((- .
user((. 2
.((2 3
Adi((3 6
}((6 7
$str((7 8
{((8 9
user((9 =
.((= >
Soyadi((> D
}((D E
"((E F
)((F G
,((G H
new)) 
Claim)) 
()) 

ClaimTypes)) $
.))$ %
Role))% )
,))) *
user))+ /
.))/ 0
IsAdmin))0 7
?))8 9
	RoleNames)): C
.))C D
Admin))D I
:))J K
())L M
user))M Q
.))Q R
Role))R V
?))V W
.))W X
Name))X \
??))] _
	RoleNames))` i
.))i j
User))j n
)))n o
)))o p
,))p q
new** 
Claim** 
(** 
JwtClaimTypes** '
.**' (
Sicil**( -
,**- .
user**/ 3
.**3 4
Sicil**4 9
??**: <
string**= C
.**C D
Empty**D I
)**I J
}++ 
;++ 
if-- 
(-- 
user-- 
.-- 
Role-- 
!=-- 
null-- !
)--! "
{.. 
claims// 
.// 
Add// 
(// 
new// 
Claim// $
(//$ %
JwtClaimTypes//% 2
.//2 3
RoleName//3 ;
,//; <
user//= A
.//A B
Role//B F
.//F G
Name//G K
)//K L
)//L M
;//M N
claims00 
.00 
Add00 
(00 
new00 
Claim00 $
(00$ %
JwtClaimTypes00% 2
.002 3
RoleId003 9
,009 :
user00; ?
.00? @
Role00@ D
.00D E
Id00E G
.00G H
ToString00H P
(00P Q
)00Q R
)00R S
)00S T
;00T U
}11 
var33 
permissions33 
=33 
await33 #
_permissionService33$ 6
.336 7'
GetPermissionsByUserIdAsync337 R
(33R S
user33S W
.33W X
Id33X Z
)33Z [
;33[ \
foreach44 
(44 
var44 

permission44 #
in44$ &
permissions44' 2
.442 3
Where443 8
(448 9
p449 :
=>44; =
p44> ?
.44? @
	IsGranted44@ I
)44I J
)44J K
{55 
claims66 
.66 
Add66 
(66 
new66 
Claim66 $
(66$ %
JwtClaimTypes66% 2
.662 3

Permission663 =
,66= >

permission66? I
.66I J
Name66J N
)66N O
)66O P
;66P Q
}77 
var99 
tokenDescriptor99 
=99  !
new99" %#
SecurityTokenDescriptor99& =
{:: 
Subject;; 
=;; 
new;; 
ClaimsIdentity;; ,
(;;, -
claims;;- 3
);;3 4
,;;4 5
Expires<< 
=<< 
DateTime<< "
.<<" #
UtcNow<<# )
.<<) *

AddMinutes<<* 4
(<<4 5
Convert<<5 <
.<<< =
ToDouble<<= E
(<<E F
_configuration<<F T
[<<T U
JwtTokenSettings<<U e
.<<e f$
TokenExpirationInMinutes<<f ~
]<<~ 
)	<< �
)
<<� �
,
<<� �
SigningCredentials== "
===# $
new==% (
SigningCredentials==) ;
(==; <
new==< ? 
SymmetricSecurityKey==@ T
(==T U
key==U X
)==X Y
,==Y Z
SecurityAlgorithms==[ m
.==m n 
HmacSha256Signature	==n �
)
==� �
,
==� �
Issuer>> 
=>> 
_configuration>> '
[>>' (
$str>>( 4
]>>4 5
,>>5 6
Audience?? 
=?? 
_configuration?? )
[??) *
$str??* 8
]??8 9
}@@ 
;@@ 
varBB 
tokenBB 
=BB 
tokenHandlerBB $
.BB$ %
CreateTokenBB% 0
(BB0 1
tokenDescriptorBB1 @
)BB@ A
;BBA B
returnCC 
tokenHandlerCC 
.CC  

WriteTokenCC  *
(CC* +
tokenCC+ 0
)CC0 1
;CC1 2
}DD 	
publicFF 
stringFF 
GenerateTokenFF #
(FF# $
UserFF$ (
userFF) -
)FF- .
{GG 	
returnHH 
GenerateTokenAsyncHH %
(HH% &
userHH& *
)HH* +
.HH+ ,

GetAwaiterHH, 6
(HH6 7
)HH7 8
.HH8 9
	GetResultHH9 B
(HHB C
)HHC D
;HHD E
}II 	
}JJ 
}KK �
`C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Services\CurrentUserService.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Services '
{ 
public 

class 
CurrentUserService #
:$ %
ICurrentUserService& 9
{ 
private		 
readonly		  
IHttpContextAccessor		 - 
_httpContextAccessor		. B
;		B C
public 
CurrentUserService !
(! " 
IHttpContextAccessor" 6
httpContextAccessor7 J
)J K
{ 	 
_httpContextAccessor  
=! "
httpContextAccessor# 6
;6 7
} 	
public 
int 
? 
UserId 
{ 	
get 
{ 
var 
userId 
=  
_httpContextAccessor 1
.1 2
HttpContext2 =
?= >
.> ?
User? C
?C D
.D E
FindFirstValueE S
(S T

ClaimTypesT ^
.^ _
NameIdentifier_ m
)m n
;n o
return 
! 
string 
. 
IsNullOrEmpty ,
(, -
userId- 3
)3 4
?5 6
int7 :
.: ;
Parse; @
(@ A
userIdA G
)G H
:I J
nullK O
;O P
} 
} 	
public 
string 
? 
UserName 
=>  " 
_httpContextAccessor# 7
.7 8
HttpContext8 C
?C D
.D E
UserE I
?I J
.J K
FindFirstValueK Y
(Y Z

ClaimTypesZ d
.d e
Namee i
)i j
;j k
public 
bool 
IsAuthenticated #
=>$ & 
_httpContextAccessor' ;
.; <
HttpContext< G
?G H
.H I
UserI M
?M N
.N O
IdentityO W
?W X
.X Y
IsAuthenticatedY h
??i k
falsel q
;q r
} 
} ��
YC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Services\AuthService.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Services '
{ 
public 

class 
AuthService 
: 
IAuthService +
{ 
private 
readonly 
IUnitOfWork $
_unitOfWork% 0
;0 1
private 
readonly 
IPasswordHasher (
_passwordHasher) 8
;8 9
private 
readonly 
IJwtTokenGenerator +
_jwtTokenGenerator, >
;> ?
private 
readonly 
ILoggerManager '
<' (
AuthService( 3
>3 4
_logger5 <
;< =
public   
AuthService   
(   
IUnitOfWork!! 

unitOfWork!! "
,!!" #
IPasswordHasher"" 
passwordHasher"" *
,""* +
IJwtTokenGenerator## 
jwtTokenGenerator## 0
,##0 1
ILoggerManager$$ 
<$$ 
AuthService$$ &
>$$& '
logger$$( .
)$$. /
{%% 	
_unitOfWork&& 
=&& 

unitOfWork&& $
;&&$ %
_passwordHasher'' 
='' 
passwordHasher'' ,
;'', -
_jwtTokenGenerator(( 
=((  
jwtTokenGenerator((! 2
;((2 3
_logger)) 
=)) 
logger)) 
;)) 
}** 	
public44 
async44 
Task44 
<44 
AuthResponseDto44 )
>44) *

LoginAsync44+ 5
(445 6
LoginDto446 >
loginDto44? G
)44G H
{55 	
_logger66 
.66 
LogInfo66 
(66 
LogMessages66 '
.66' (
LoginAttempt66( 4
,664 5
loginDto666 >
.66> ?
Sicil66? D
,66D E
loginDto66F N
.66N O
Password66O W
?66W X
.66X Y
Length66Y _
??66` b
$num66c d
)66d e
;66e f
var88 
userSpec88 
=88 
new88 $
UserBySicilSpecification88 7
(887 8
loginDto888 @
.88@ A
Sicil88A F
,88F G
includeRole88H S
:88S T
true88U Y
)88Y Z
;88Z [
var99 
user99 
=99 
await99 
_unitOfWork99 (
.99( )
Users99) .
.99. /
FirstOrDefaultAsync99/ B
(99B C
userSpec99C K
)99K L
;99L M
if;; 
(;; 
user;; 
==;; 
null;; 
);; 
{<< 
_logger== 
.== 
LogWarn== 
(==  
LogMessages==  +
.==+ ,"
LogUserNotFoundBySicil==, B
,==B C
loginDto==D L
.==L M
Sicil==M R
)==R S
;==S T
return>> 
new>> 
AuthResponseDto>> *
{?? 
Success@@ 
=@@ 
false@@ #
,@@# $
ErrorMessageAA  
=AA! "
ErrorMessagesAA# 0
.AA0 1
InvalidCredentialsAA1 C
}BB 
;BB 
}CC 
_loggerEE 
.EE 
LogInfoEE 
(EE 
LogMessagesEE '
.EE' (
	UserFoundEE( 1
,EE1 2
userEE3 7
.EE7 8
SicilEE8 =
,EE= >
userEE? C
.EEC D
IdEED F
,EEF G
userEEH L
.EEL M
IsAdminEEM T
)EET U
;EEU V
_loggerFF 
.FF 
LogInfoFF 
(FF 
LogMessagesFF '
.FF' ((
PasswordVerificationStartingFF( D
,FFD E
userFFF J
.FFJ K
PasswordHashFFK W
)FFW X
;FFX Y
_loggerGG 
.GG 
LogInfoGG 
(GG 
LogMessagesGG '
.GG' (

HashLengthGG( 2
,GG2 3
userGG4 8
.GG8 9
PasswordHashGG9 E
?GGE F
.GGF G
LengthGGG M
??GGN P
$numGGQ R
)GGR S
;GGS T
_loggerHH 
.HH 
LogInfoHH 
(HH 
LogMessagesHH '
.HH' (

HashFormatHH( 2
,HH2 3
(HH4 5
userHH5 9
.HH9 :
PasswordHashHH: F
?HHF G
.HHG H

StartsWithHHH R
(HHR S
$strHHS W
)HHW X
==HHY [
trueHH\ `
?HHa b
$strHHc k
:HHl m
$strHHn z
)HHz {
)HH{ |
;HH| }
boolJJ 
isPasswordValidJJ  
=JJ! "
falseJJ# (
;JJ( )
tryKK 
{LL 
ifMM 
(MM 
userMM 
.MM 
PasswordHashMM %
!=MM& (
nullMM) -
&&MM. 0
loginDtoMM1 9
.MM9 :
PasswordMM: B
!=MMC E
nullMMF J
)MMJ K
{NN 
isPasswordValidOO #
=OO$ %
_passwordHasherOO& 5
.OO5 6
VerifyPasswordOO6 D
(OOD E
loginDtoOOE M
.OOM N
PasswordOON V
,OOV W
userOOX \
.OO\ ]
PasswordHashOO] i
)OOi j
;OOj k
}PP 
_loggerQQ 
.QQ 
LogInfoQQ 
(QQ  
LogMessagesQQ  +
.QQ+ ,&
PasswordVerificationResultQQ, F
,QQF G
isPasswordValidQQH W
)QQW X
;QQX Y
}RR 
catchSS 
(SS 
	ExceptionSS 
exSS 
)SS  
{TT 
_loggerUU 
.UU 
LogErrorUU  
(UU  !
exUU! #
,UU# $
LogMessagesUU% 0
.UU0 1%
PasswordVerificationErrorUU1 J
)UUJ K
;UUK L
returnVV 
newVV 
AuthResponseDtoVV *
{WW 
SuccessXX 
=XX 
falseXX #
,XX# $
ErrorMessageYY  
=YY! "
ErrorMessagesYY# 0
.YY0 1&
PasswordVerificationFailedYY1 K
}ZZ 
;ZZ 
}[[ 
if]] 
(]] 
!]] 
isPasswordValid]]  
)]]  !
{^^ 
_logger__ 
.__ 
LogWarn__ 
(__  
LogMessages__  +
.__+ ,%
LogInvalidPasswordAttempt__, E
,__E F
loginDto__G O
.__O P
Sicil__P U
)__U V
;__V W
return`` 
new`` 
AuthResponseDto`` *
{aa 
Successbb 
=bb 
falsebb #
,bb# $
ErrorMessagecc  
=cc! "
ErrorMessagescc# 0
.cc0 1
InvalidCredentialscc1 C
}dd 
;dd 
}ee 
vargg 
tokengg 
=gg 
_jwtTokenGeneratorgg *
.gg* +
GenerateTokengg+ 8
(gg8 9
usergg9 =
)gg= >
;gg> ?
_loggerhh 
.hh 
LogInfohh 
(hh 
LogMessageshh '
.hh' (
LogLoginSuccesshh( 7
,hh7 8
loginDtohh9 A
.hhA B
SicilhhB G
)hhG H
;hhH I
returnjj 
newjj 
AuthResponseDtojj &
{kk 
Successll 
=ll 
truell 
,ll 
Tokenmm 
=mm 
tokenmm 
,mm 
Adinn 
=nn 
usernn 
.nn 
Adinn 
,nn 
Soyadioo 
=oo 
useroo 
.oo 
Soyadioo $
,oo$ %
IsAdminpp 
=pp 
userpp 
.pp 
IsAdminpp &
,pp& '
RoleNameqq 
=qq 
userqq 
.qq  
Roleqq  $
?qq$ %
.qq% &
Nameqq& *
}rr 
;rr 
}ss 	
public}} 
async}} 
Task}} 
<}} 
AuthResponseDto}} )
>}}) *
RegisterAsync}}+ 8
(}}8 9
RegisterDto}}9 D
registerDto}}E P
)}}P Q
{~~ 	
_logger 
. 
LogInfo 
( 
LogMessages '
.' (
LogRegisterAttempt( :
,: ;
registerDto< G
.G H
SicilH M
)M N
;N O
var
�� 
existingUserSpec
��  
=
��! "
new
��# &&
UserBySicilSpecification
��' ?
(
��? @
registerDto
��@ K
.
��K L
Sicil
��L Q
,
��Q R
includeRole
��S ^
:
��^ _
false
��` e
)
��e f
;
��f g
var
�� 
existingUser
�� 
=
�� 
await
�� $
_unitOfWork
��% 0
.
��0 1
Users
��1 6
.
��6 7!
FirstOrDefaultAsync
��7 J
(
��J K
existingUserSpec
��K [
)
��[ \
;
��\ ]
if
�� 
(
�� 
existingUser
�� 
!=
�� 
null
��  $
)
��$ %
{
�� 
_logger
�� 
.
�� 
LogWarn
�� 
(
��  
LogMessages
��  +
.
��+ ,#
LogSicilAlreadyExists
��, A
,
��A B
registerDto
��C N
.
��N O
Sicil
��O T
)
��T U
;
��U V
return
�� 
new
�� 
AuthResponseDto
�� *
{
�� 
Success
�� 
=
�� 
false
�� #
,
��# $
ErrorMessage
��  
=
��! "
ErrorMessages
��# 0
.
��0 1 
SicilAlreadyExists
��1 C
}
�� 
;
�� 
}
�� 
try
�� 
{
�� 
var
�� 
passwordHash
��  
=
��! "
_passwordHasher
��# 2
.
��2 3
HashPassword
��3 ?
(
��? @
registerDto
��@ K
.
��K L
Password
��L T
)
��T U
;
��U V
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,'
LogPasswordHashedForSicil
��, E
,
��E F
registerDto
��G R
.
��R S
Sicil
��S X
,
��X Y
passwordHash
��Z f
)
��f g
;
��g h
bool
�� 
isAdmin
�� 
=
�� 
registerDto
�� *
.
��* +
RoleId
��+ 1
.
��1 2
HasValue
��2 :
&&
��; =
registerDto
��> I
.
��I J
RoleId
��J P
.
��P Q
Value
��Q V
==
��W Y
$num
��Z [
;
��[ \
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,"
UserAdminStatusCheck
��, @
,
��@ A
isAdmin
��B I
,
��I J
registerDto
��K V
.
��V W
RoleId
��W ]
)
��] ^
;
��^ _
var
�� 
user
�� 
=
�� 
new
�� 
User
�� #
{
�� 
Adi
�� 
=
�� 
registerDto
�� %
.
��% &
Adi
��& )
,
��) *
Soyadi
�� 
=
�� 
registerDto
�� (
.
��( )
Soyadi
��) /
,
��/ 0
PasswordHash
��  
=
��! "
passwordHash
��# /
,
��/ 0
Sicil
�� 
=
�� 
registerDto
�� '
.
��' (
Sicil
��( -
,
��- .
IsAdmin
�� 
=
�� 
isAdmin
�� %
,
��% &
RoleId
�� 
=
�� 
registerDto
�� (
.
��( )
RoleId
��) /
}
�� 
;
�� 
await
�� 
_unitOfWork
�� !
.
��! "
Users
��" '
.
��' (
AddAsync
��( 0
(
��0 1
user
��1 5
)
��5 6
;
��6 7
await
�� 
_unitOfWork
�� !
.
��! "
SaveChangesAsync
��" 2
(
��2 3
)
��3 4
;
��4 5
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,(
LogUserRegisteredWithSicil
��, F
,
��F G
registerDto
��H S
.
��S T
Sicil
��T Y
,
��Y Z
user
��[ _
.
��_ `
Id
��` b
,
��b c
user
��d h
.
��h i
IsAdmin
��i p
)
��p q
;
��q r
var
��  
registeredUserSpec
�� &
=
��' (
new
��) ,%
EntityByIdSpecification
��- D
<
��D E
User
��E I
>
��I J
(
��J K
user
��K O
.
��O P
Id
��P R
,
��R S
u
��T U
=>
��V X
u
��Y Z
.
��Z [
Role
��[ _
)
��_ `
;
��` a
var
�� 
registeredUser
�� "
=
��# $
await
��% *
_unitOfWork
��+ 6
.
��6 7
Users
��7 <
.
��< =!
FirstOrDefaultAsync
��= P
(
��P Q 
registeredUserSpec
��Q c
)
��c d
;
��d e
var
�� 
token
�� 
=
��  
_jwtTokenGenerator
�� .
.
��. /
GenerateToken
��/ <
(
��< =
registeredUser
��= K
??
��L N
user
��O S
)
��S T
;
��T U
_logger
�� 
.
�� 
LogInfo
�� 
(
��  
LogMessages
��  +
.
��+ ,'
LogTokenGeneratedForSicil
��, E
,
��E F
registerDto
��G R
.
��R S
Sicil
��S X
??
��Y [
$str
��\ j
)
��j k
;
��k l
return
�� 
new
�� 
AuthResponseDto
�� *
{
�� 
Success
�� 
=
�� 
true
�� "
,
��" #
Token
�� 
=
�� 
token
�� !
,
��! "
Adi
�� 
=
�� 
user
�� 
.
�� 
Adi
�� "
,
��" #
Soyadi
�� 
=
�� 
user
�� !
.
��! "
Soyadi
��" (
,
��( )
IsAdmin
�� 
=
�� 
user
�� "
.
��" #
IsAdmin
��# *
,
��* +
RoleName
�� 
=
�� 
(
��  
registeredUser
��  .
?
��. /
.
��/ 0
Role
��0 4
!=
��5 7
null
��8 <
)
��< =
?
��> ?
registeredUser
��@ N
.
��N O
Role
��O S
.
��S T
Name
��T X
:
��Y Z
null
��[ _
}
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
��  
{
�� 
_logger
�� 
.
�� 
LogError
��  
(
��  !
ex
��! #
,
��# $
LogMessages
��% 0
.
��0 1+
LogRegistrationErrorWithSicil
��1 N
,
��N O
registerDto
��P [
.
��[ \
Sicil
��\ a
)
��a b
;
��b c
return
�� 
new
�� 
AuthResponseDto
�� *
{
�� 
Success
�� 
=
�� 
false
�� #
,
��# $
ErrorMessage
��  
=
��! "
ErrorMessages
��# 0
.
��0 1 
RegistrationFailed
��1 C
}
�� 
;
�� 
}
�� 
}
�� 	
public
�� 
string
�� 
GenerateJwtToken
�� &
(
��& '
UserDto
��' .
user
��/ 3
)
��3 4
{
�� 	
var
�� 

userEntity
�� 
=
�� 
new
��  
User
��! %
{
�� 
Id
�� 
=
�� 
user
�� 
.
�� 
Id
�� 
,
�� 
Adi
�� 
=
�� 
user
�� 
.
�� 
Adi
�� 
,
�� 
Soyadi
�� 
=
�� 
user
�� 
.
�� 
Soyadi
�� $
,
��$ %
IsAdmin
�� 
=
�� 
user
�� 
.
�� 
IsAdmin
�� &
,
��& '
RoleId
�� 
=
�� 
user
�� 
.
�� 
RoleId
�� $
,
��$ %
Sicil
�� 
=
�� 
user
�� 
.
�� 
Sicil
�� "
}
�� 
;
�� 
if
�� 
(
�� 
user
�� 
.
�� 
RoleId
�� 
.
�� 
HasValue
�� $
&&
��% '
!
��( )
string
��) /
.
��/ 0
IsNullOrEmpty
��0 =
(
��= >
user
��> B
.
��B C
RoleName
��C K
)
��K L
)
��L M
{
�� 

userEntity
�� 
.
�� 
Role
�� 
=
��  !
new
��" %
Role
��& *
{
�� 
Id
�� 
=
�� 
user
�� 
.
�� 
RoleId
�� $
.
��$ %
Value
��% *
,
��* +
Name
�� 
=
�� 
user
�� 
.
��  
RoleName
��  (
}
�� 
;
�� 
}
�� 
return
��  
_jwtTokenGenerator
�� %
.
��% &
GenerateToken
��& 3
(
��3 4

userEntity
��4 >
)
��> ?
;
��? @
}
�� 	
public
�� 
async
�� 
Task
�� 
<
�� 
string
��  
>
��  !#
GenerateJwtTokenAsync
��" 7
(
��7 8
UserDto
��8 ?
user
��@ D
)
��D E
{
�� 	
var
�� 

userEntity
�� 
=
�� 
new
��  
User
��! %
{
�� 
Id
�� 
=
�� 
user
�� 
.
�� 
Id
�� 
,
�� 
Adi
�� 
=
�� 
user
�� 
.
�� 
Adi
�� 
,
�� 
Soyadi
�� 
=
�� 
user
�� 
.
�� 
Soyadi
�� $
,
��$ %
IsAdmin
�� 
=
�� 
user
�� 
.
�� 
IsAdmin
�� &
,
��& '
RoleId
�� 
=
�� 
user
�� 
.
�� 
RoleId
�� $
,
��$ %
Sicil
�� 
=
�� 
user
�� 
.
�� 
Sicil
�� "
}
�� 
;
�� 
if
�� 
(
�� 
user
�� 
.
�� 
RoleId
�� 
.
�� 
HasValue
�� $
&&
��% '
!
��( )
string
��) /
.
��/ 0
IsNullOrEmpty
��0 =
(
��= >
user
��> B
.
��B C
RoleName
��C K
)
��K L
)
��L M
{
�� 

userEntity
�� 
.
�� 
Role
�� 
=
��  !
new
��" %
Role
��& *
{
�� 
Id
�� 
=
�� 
user
�� 
.
�� 
RoleId
�� $
.
��$ %
Value
��% *
,
��* +
Name
�� 
=
�� 
user
�� 
.
��  
RoleName
��  (
}
�� 
;
�� 
}
�� 
return
�� 
await
��  
_jwtTokenGenerator
�� +
.
��+ , 
GenerateTokenAsync
��, >
(
��> ?

userEntity
��? I
)
��I J
;
��J K
}
�� 	
public
�� 
async
�� 
Task
�� 
<
�� 
AuthResponseDto
�� )
>
��) *!
ChangePasswordAsync
��+ >
(
��> ?
ChangePasswordDto
��? P
changePasswordDto
��Q b
,
��b c
int
��d g
userId
��h n
)
��n o
{
�� 	
_logger
�� 
.
�� 
LogInfo
�� 
(
�� 
LogMessages
�� '
.
��' (#
ChangePasswordAttempt
��( =
,
��= >
userId
��? E
)
��E F
;
��F G
var
�� 
spec
�� 
=
�� 
new
�� %
EntityByIdSpecification
�� 2
<
��2 3
User
��3 7
>
��7 8
(
��8 9
userId
��9 ?
)
��? @
;
��@ A
var
�� 
user
�� 
=
�� 
await
�� 
_unitOfWork
�� (
.
��( )
Users
��) .
.
��. /!
FirstOrDefaultAsync
��/ B
(
��B C
spec
��C G
)
��G H
;
��H I
if
�� 
(
�� 
user
�� 
==
�� 
null
�� 
)
�� 
{
�� 
_logger
�� 
.
�� 
LogWarn
�� 
(
��  
LogMessages
��  +
.
��+ ,
UserNotFoundById
��, <
,
��< =
userId
��> D
)
��D E
;
��E F
return
�� 
new
�� 
AuthResponseDto
�� *
{
�� 
Success
�� 
=
�� 
false
�� #
,
��# $
ErrorMessage
��  
=
��! "
ErrorMessages
��# 0
.
��0 1
UserNotFound
��1 =
}
�� 
;
�� 
}
�� 
if
�� 
(
�� 
!
�� 
_passwordHasher
��  
.
��  !
VerifyPassword
��! /
(
��/ 0
changePasswordDto
��0 A
.
��A B
CurrentPassword
��B Q
,
��Q R
user
��S W
.
��W X
PasswordHash
��X d
)
��d e
)
��e f
{
�� 
_logger
�� 
.
�� 
LogWarn
�� 
(
��  
LogMessages
��  +
.
��+ ,"
IncorrectOldPassword
��, @
,
��@ A
userId
��B H
)
��H I
;
��I J
return
�� 
new
�� 
AuthResponseDto
�� *
{
�� 
Success
�� 
=
�� 
false
�� #
,
��# $
ErrorMessage
��  
=
��! "
ErrorMessages
��# 0
.
��0 1
IncorrectPassword
��1 B
}
�� 
;
�� 
}
�� 
user
�� 
.
�� 
PasswordHash
�� 
=
�� 
_passwordHasher
��  /
.
��/ 0
HashPassword
��0 <
(
��< =
changePasswordDto
��= N
.
��N O
NewPassword
��O Z
)
��Z [
;
��[ \
user
�� 
.
�� 
	UpdatedAt
�� 
=
�� 
DateTime
�� %
.
��% &
UtcNow
��& ,
;
��, -
_unitOfWork
�� 
.
�� 
Users
�� 
.
�� 
Update
�� $
(
��$ %
user
��% )
)
��) *
;
��* +
await
�� 
_unitOfWork
�� 
.
�� 
SaveChangesAsync
�� .
(
��. /
)
��/ 0
;
��0 1
_logger
�� 
.
�� 
LogInfo
�� 
(
�� 
LogMessages
�� '
.
��' ()
PasswordChangedSuccessfully
��( C
,
��C D
userId
��E K
)
��K L
;
��L M
return
�� 
new
�� 
AuthResponseDto
�� &
{
��' (
Success
��) 0
=
��1 2
true
��3 7
}
��8 9
;
��9 :
}
�� 	
}
�� 
}�� �
ZC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Services\AuditService.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Services '
{ 
public		 

class		 
AuditService		 
:		 
IAuditService		  -
{

 
private 
readonly 
IUnitOfWork $
_unitOfWork% 0
;0 1
public 
AuditService 
( 
IUnitOfWork '

unitOfWork( 2
)2 3
{ 	
_unitOfWork 
= 

unitOfWork $
;$ %
} 	
public 
async 
Task 
LogActionAsync (
(( )
string) /
action0 6
,6 7
string8 >

entityType? I
,I J
stringK Q
entityIdR Z
,Z [
int\ _
?_ `
userIda g
=h i
nullj n
,n o
stringp v
?v w
pathx |
=} ~
null	 �
,
� �
string
� �
?
� �
details
� �
=
� �
null
� �
)
� �
{ 	
var 
auditLog 
= 
new 
AuditLog '
{ 
Action 
= 
action 
,  

EntityType 
= 

entityType '
,' (
EntityId 
= 
entityId #
,# $
UserId 
= 
userId 
,  
Path 
= 
path 
, 
Details 
= 
details !
,! "
	CreatedAt 
= 
DateTime $
.$ %
UtcNow% +
} 
; 
await 
_unitOfWork 
. 
GetRepository +
<+ ,
AuditLog, 4
>4 5
(5 6
)6 7
.7 8
AddAsync8 @
(@ A
auditLogA I
)I J
;J K
await   
_unitOfWork   
.   
SaveChangesAsync   .
(  . /
)  / 0
;  0 1
}!! 	
}"" 
}## �
`C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Repositories\UserRepository.cs
	namespace

 	
Stock


 
.

 
Infrastructure

 
.

 
Repositories

 +
{ 
public 

class 
UserRepository 
:  !
GenericRepository" 3
<3 4
User4 8
>8 9
,9 :
IUserRepository; J
{ 
public 
UserRepository 
(  
ApplicationDbContext 2
context3 :
): ;
:< =
base> B
(B C
contextC J
)J K
{ 	
} 	
public 
async 
Task 
< 
User 
? 
>  
GetBySicilAsync! 0
(0 1
string1 7
sicil8 =
)= >
{ 	
return 
await 
_dbSet 
. 
Include 
( 
u 
=> 
u 
.  
Role  $
)$ %
. 
FirstOrDefaultAsync $
($ %
u% &
=>' )
u* +
.+ ,
Sicil, 1
==2 4
sicil5 :
&&; =
!> ?
u? @
.@ A
	IsDeletedA J
)J K
;K L
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
User& *
>* +
>+ ,"
GetUsersWithRolesAsync- C
(C D
)D E
{ 	
return 
await 
_dbSet 
. 
Include 
( 
u 
=> 
u 
.  
Role  $
)$ %
. 
Where 
( 
u 
=> 
! 
u 
. 
	IsDeleted (
)( )
. 
ToListAsync 
( 
) 
; 
} 	
}   
}!! �
`C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Repositories\RoleRepository.cs
	namespace

 	
Stock


 
.

 
Infrastructure

 
.

 
Repositories

 +
{ 
public 

class 
RoleRepository 
:  !
GenericRepository" 3
<3 4
Role4 8
>8 9
,9 :
IRoleRepository; J
{ 
public 
RoleRepository 
(  
ApplicationDbContext 2
context3 :
): ;
:< =
base> B
(B C
contextC J
)J K
{ 	
} 	
public 
async 
Task 
< 
Role 
? 
>  
GetByNameAsync! /
(/ 0
string0 6
name7 ;
); <
{ 	
return 
await 
_dbSet 
. 
FirstOrDefaultAsync $
($ %
r% &
=>' )
r* +
.+ ,
Name, 0
==1 3
name4 8
&&9 ;
!< =
r= >
.> ?
	IsDeleted? H
)H I
;I J
} 	
public 
async 
Task 
< 
IEnumerable %
<% &
Role& *
>* +
>+ ,"
GetRolesWithUsersAsync- C
(C D
)D E
{ 	
return 
await 
_dbSet 
. 
Include 
( 
r 
=> 
r 
.  
Users  %
)% &
. 
Where 
( 
r 
=> 
! 
r 
. 
	IsDeleted (
)( )
. 
ToListAsync 
( 
) 
; 
} 	
} 
}   �$
fC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Repositories\PermissionRepository.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Repositories +
{ 
public 

class  
PermissionRepository %
:& '
GenericRepository( 9
<9 :

Permission: D
>D E
,E F!
IPermissionRepositoryG \
{ 
public  
PermissionRepository #
(# $ 
ApplicationDbContext$ 8
context9 @
)@ A
:B C
baseD H
(H I
contextI P
)P Q
{ 	
} 	
public 
async 
Task 
< 

Permission $
?$ %
>% &
GetByNameAsync' 5
(5 6
string6 <
name= A
)A B
{ 	
return 
await 
_context !
.! "
Permissions" -
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
Name, 0
==1 3
name4 8
)8 9
;9 :
} 	
public 
async 
Task 
< 
IEnumerable %
<% &

Permission& 0
>0 1
>1 2&
GetPermissionsByGroupAsync3 M
(M N
stringN T
groupU Z
)Z [
{ 	
return 
await 
_context !
.! "
Permissions" -
. 
Where 
( 
p 
=> 
p 
. 
Group #
==$ &
group' ,
), -
. 
ToListAsync 
( 
) 
; 
} 	
public!! 
async!! 
Task!! 
<!! 
IEnumerable!! %
<!!% &

Permission!!& 0
>!!0 1
>!!1 2'
GetPermissionsByRoleIdAsync!!3 N
(!!N O
int!!O R
roleId!!S Y
)!!Y Z
{"" 	
return## 
await## 
_context## !
.##! "
RolePermissions##" 1
.$$ 
Where$$ 
($$ 
rp$$ 
=>$$ 
rp$$ 
.$$  
RoleId$$  &
==$$' )
roleId$$* 0
)$$0 1
.%% 
Include%% 
(%% 
rp%% 
=>%% 
rp%% !
.%%! "

Permission%%" ,
)%%, -
.&& 
Select&& 
(&& 
rp&& 
=>&& 
rp&&  
.&&  !

Permission&&! +
)&&+ ,
.'' 
ToListAsync'' 
('' 
)'' 
;'' 
}(( 	
public** 
async** 
Task** &
RemoveRolePermissionsAsync** 4
(**4 5
int**5 8
roleId**9 ?
)**? @
{++ 	
var,, #
existingRolePermissions,, '
=,,( )
await,,* /
_context,,0 8
.,,8 9
RolePermissions,,9 H
.-- 
Where-- 
(-- 
rp-- 
=>-- 
rp-- 
.--  
RoleId--  &
==--' )
roleId--* 0
)--0 1
... 
ToListAsync.. 
(.. 
).. 
;.. 
_context00 
.00 
RolePermissions00 $
.00$ %
RemoveRange00% 0
(000 1#
existingRolePermissions001 H
)00H I
;00I J
}11 	
public33 
async33 
Task33 #
AddRolePermissionsAsync33 1
(331 2
int332 5
roleId336 <
,33< =
List33> B
<33B C
int33C F
>33F G
permissionIds33H U
)33U V
{44 	
foreach55 
(55 
var55 
permissionId55 %
in55& (
permissionIds55) 6
)556 7
{66 
var77 
rolePermission77 "
=77# $
new77% (
RolePermission77) 7
{88 
RoleId99 
=99 
roleId99 #
,99# $
PermissionId::  
=::! "
permissionId::# /
};; 
;;; 
await== 
_context== 
.== 
RolePermissions== .
.==. /
AddAsync==/ 7
(==7 8
rolePermission==8 F
)==F G
;==G H
}>> 
}?? 	
}@@ 
}AA ��
lC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Migrations\20250426182949_InitialCreate.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 

Migrations )
{ 
public

 

partial

 
class

 
InitialCreate

 &
:

' (
	Migration

) 2
{ 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str "
," #
columns 
: 
table 
=> !
new" %
{ 
Id 
= 
table 
. 
Column %
<% &
int& )
>) *
(* +
type+ /
:/ 0
$str1 :
,: ;
nullable< D
:D E
falseF K
)K L
. 

Annotation #
(# $
$str$ D
,D E)
NpgsqlValueGenerationStrategyF c
.c d#
IdentityByDefaultColumnd {
){ |
,| }
Name 
= 
table  
.  !
Column! '
<' (
string( .
>. /
(/ 0
type0 4
:4 5
$str6 N
,N O
	maxLengthP Y
:Y Z
$num[ ^
,^ _
nullable` h
:h i
falsej o
)o p
,p q
Description 
=  !
table" '
.' (
Column( .
<. /
string/ 5
>5 6
(6 7
type7 ;
:; <
$str= U
,U V
	maxLengthW `
:` a
$numb e
,e f
nullableg o
:o p
falseq v
)v w
,w x
	CreatedAt 
= 
table  %
.% &
Column& ,
<, -
DateTime- 5
>5 6
(6 7
type7 ;
:; <
$str= W
,W X
nullableY a
:a b
falsec h
)h i
,i j
	UpdatedAt 
= 
table  %
.% &
Column& ,
<, -
DateTime- 5
>5 6
(6 7
type7 ;
:; <
$str= W
,W X
nullableY a
:a b
truec g
)g h
,h i
	CreatedBy 
= 
table  %
.% &
Column& ,
<, -
string- 3
>3 4
(4 5
type5 9
:9 :
$str; A
,A B
nullableC K
:K L
trueM Q
)Q R
,R S
	UpdatedBy 
= 
table  %
.% &
Column& ,
<, -
string- 3
>3 4
(4 5
type5 9
:9 :
$str; A
,A B
nullableC K
:K L
trueM Q
)Q R
,R S
	IsDeleted 
= 
table  %
.% &
Column& ,
<, -
bool- 1
>1 2
(2 3
type3 7
:7 8
$str9 B
,B C
nullableD L
:L M
falseN S
)S T
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% 4
,4 5
x6 7
=>8 :
x; <
.< =
Id= ?
)? @
;@ A
}   
)   
;   
migrationBuilder"" 
."" 
CreateTable"" (
(""( )
name## 
:## 
$str## #
,### $
columns$$ 
:$$ 
table$$ 
=>$$ !
new$$" %
{%% 
Id&& 
=&& 
table&& 
.&& 
Column&& %
<&&% &
int&&& )
>&&) *
(&&* +
type&&+ /
:&&/ 0
$str&&1 :
,&&: ;
nullable&&< D
:&&D E
false&&F K
)&&K L
.'' 

Annotation'' #
(''# $
$str''$ D
,''D E)
NpgsqlValueGenerationStrategy''F c
.''c d#
IdentityByDefaultColumn''d {
)''{ |
,''| }
Name(( 
=(( 
table((  
.((  !
Column((! '
<((' (
string((( .
>((. /
(((/ 0
type((0 4
:((4 5
$str((6 M
,((M N
	maxLength((O X
:((X Y
$num((Z \
,((\ ]
nullable((^ f
:((f g
false((h m
)((m n
,((n o
Description)) 
=))  !
table))" '
.))' (
Column))( .
<)). /
string))/ 5
>))5 6
())6 7
type))7 ;
:)); <
$str))= U
,))U V
	maxLength))W `
:))` a
$num))b e
,))e f
nullable))g o
:))o p
false))q v
)))v w
,))w x
ResourceType**  
=**! "
table**# (
.**( )
Column**) /
<**/ 0
string**0 6
>**6 7
(**7 8
type**8 <
:**< =
$str**> U
,**U V
	maxLength**W `
:**` a
$num**b d
,**d e
nullable**f n
:**n o
false**p u
)**u v
,**v w
ResourceName++  
=++! "
table++# (
.++( )
Column++) /
<++/ 0
string++0 6
>++6 7
(++7 8
type++8 <
:++< =
$str++> U
,++U V
	maxLength++W `
:++` a
$num++b d
,++d e
nullable++f n
:++n o
false++p u
)++u v
,++v w
Action,, 
=,, 
table,, "
.,," #
Column,,# )
<,,) *
string,,* 0
>,,0 1
(,,1 2
type,,2 6
:,,6 7
$str,,8 O
,,,O P
	maxLength,,Q Z
:,,Z [
$num,,\ ^
,,,^ _
nullable,,` h
:,,h i
false,,j o
),,o p
,,,p q
Group-- 
=-- 
table-- !
.--! "
Column--" (
<--( )
string--) /
>--/ 0
(--0 1
type--1 5
:--5 6
$str--7 N
,--N O
	maxLength--P Y
:--Y Z
$num--[ ]
,--] ^
nullable--_ g
:--g h
false--i n
)--n o
,--o p
	CreatedAt.. 
=.. 
table..  %
...% &
Column..& ,
<.., -
DateTime..- 5
>..5 6
(..6 7
type..7 ;
:..; <
$str..= W
,..W X
nullable..Y a
:..a b
false..c h
)..h i
,..i j
	UpdatedAt// 
=// 
table//  %
.//% &
Column//& ,
<//, -
DateTime//- 5
>//5 6
(//6 7
type//7 ;
://; <
$str//= W
,//W X
nullable//Y a
://a b
true//c g
)//g h
,//h i
	CreatedBy00 
=00 
table00  %
.00% &
Column00& ,
<00, -
string00- 3
>003 4
(004 5
type005 9
:009 :
$str00; A
,00A B
nullable00C K
:00K L
true00M Q
)00Q R
,00R S
	UpdatedBy11 
=11 
table11  %
.11% &
Column11& ,
<11, -
string11- 3
>113 4
(114 5
type115 9
:119 :
$str11; A
,11A B
nullable11C K
:11K L
true11M Q
)11Q R
,11R S
	IsDeleted22 
=22 
table22  %
.22% &
Column22& ,
<22, -
bool22- 1
>221 2
(222 3
type223 7
:227 8
$str229 B
,22B C
nullable22D L
:22L M
false22N S
,22S T
defaultValue22U a
:22a b
false22c h
)22h i
}33 
,33 
constraints44 
:44 
table44 "
=>44# %
{55 
table66 
.66 

PrimaryKey66 $
(66$ %
$str66% 5
,665 6
x667 8
=>669 ;
x66< =
.66= >
Id66> @
)66@ A
;66A B
}77 
)77 
;77 
migrationBuilder99 
.99 
CreateTable99 (
(99( )
name:: 
::: 
$str:: 
,:: 
columns;; 
:;; 
table;; 
=>;; !
new;;" %
{<< 
Id== 
=== 
table== 
.== 
Column== %
<==% &
int==& )
>==) *
(==* +
type==+ /
:==/ 0
$str==1 :
,==: ;
nullable==< D
:==D E
false==F K
)==K L
.>> 

Annotation>> #
(>># $
$str>>$ D
,>>D E)
NpgsqlValueGenerationStrategy>>F c
.>>c d#
IdentityByDefaultColumn>>d {
)>>{ |
,>>| }
Name?? 
=?? 
table??  
.??  !
Column??! '
<??' (
string??( .
>??. /
(??/ 0
type??0 4
:??4 5
$str??6 M
,??M N
	maxLength??O X
:??X Y
$num??Z \
,??\ ]
nullable??^ f
:??f g
false??h m
)??m n
,??n o
Description@@ 
=@@  !
table@@" '
.@@' (
Column@@( .
<@@. /
string@@/ 5
>@@5 6
(@@6 7
type@@7 ;
:@@; <
$str@@= U
,@@U V
	maxLength@@W `
:@@` a
$num@@b e
,@@e f
nullable@@g o
:@@o p
false@@q v
)@@v w
,@@w x
	CreatedAtAA 
=AA 
tableAA  %
.AA% &
ColumnAA& ,
<AA, -
DateTimeAA- 5
>AA5 6
(AA6 7
typeAA7 ;
:AA; <
$strAA= W
,AAW X
nullableAAY a
:AAa b
falseAAc h
)AAh i
,AAi j
	UpdatedAtBB 
=BB 
tableBB  %
.BB% &
ColumnBB& ,
<BB, -
DateTimeBB- 5
>BB5 6
(BB6 7
typeBB7 ;
:BB; <
$strBB= W
,BBW X
nullableBBY a
:BBa b
trueBBc g
)BBg h
,BBh i
	CreatedByCC 
=CC 
tableCC  %
.CC% &
ColumnCC& ,
<CC, -
stringCC- 3
>CC3 4
(CC4 5
typeCC5 9
:CC9 :
$strCC; R
,CCR S
	maxLengthCCT ]
:CC] ^
$numCC_ a
,CCa b
nullableCCc k
:CCk l
trueCCm q
)CCq r
,CCr s
	UpdatedByDD 
=DD 
tableDD  %
.DD% &
ColumnDD& ,
<DD, -
stringDD- 3
>DD3 4
(DD4 5
typeDD5 9
:DD9 :
$strDD; R
,DDR S
	maxLengthDDT ]
:DD] ^
$numDD_ a
,DDa b
nullableDDc k
:DDk l
trueDDm q
)DDq r
,DDr s
	IsDeletedEE 
=EE 
tableEE  %
.EE% &
ColumnEE& ,
<EE, -
boolEE- 1
>EE1 2
(EE2 3
typeEE3 7
:EE7 8
$strEE9 B
,EEB C
nullableEED L
:EEL M
falseEEN S
,EES T
defaultValueEEU a
:EEa b
falseEEc h
)EEh i
}FF 
,FF 
constraintsGG 
:GG 
tableGG "
=>GG# %
{HH 
tableII 
.II 

PrimaryKeyII $
(II$ %
$strII% /
,II/ 0
xII1 2
=>II3 5
xII6 7
.II7 8
IdII8 :
)II: ;
;II; <
}JJ 
)JJ 
;JJ 
migrationBuilderLL 
.LL 
CreateTableLL (
(LL( )
nameMM 
:MM 
$strMM  
,MM  !
columnsNN 
:NN 
tableNN 
=>NN !
newNN" %
{OO 
IdPP 
=PP 
tablePP 
.PP 
ColumnPP %
<PP% &
intPP& )
>PP) *
(PP* +
typePP+ /
:PP/ 0
$strPP1 :
,PP: ;
nullablePP< D
:PPD E
falsePPF K
)PPK L
.QQ 

AnnotationQQ #
(QQ# $
$strQQ$ D
,QQD E)
NpgsqlValueGenerationStrategyQQF c
.QQc d#
IdentityByDefaultColumnQQd {
)QQ{ |
,QQ| }

CategoryIdRR 
=RR  
tableRR! &
.RR& '
ColumnRR' -
<RR- .
intRR. 1
>RR1 2
(RR2 3
typeRR3 7
:RR7 8
$strRR9 B
,RRB C
nullableRRD L
:RRL M
falseRRN S
)RRS T
,RRT U
CategoryId1SS 
=SS  !
tableSS" '
.SS' (
ColumnSS( .
<SS. /
intSS/ 2
>SS2 3
(SS3 4
typeSS4 8
:SS8 9
$strSS: C
,SSC D
nullableSSE M
:SSM N
trueSSO S
)SSS T
,SST U
DescriptionTT 
=TT  !
tableTT" '
.TT' (
ColumnTT( .
<TT. /
stringTT/ 5
>TT5 6
(TT6 7
typeTT7 ;
:TT; <
$strTT= U
,TTU V
	maxLengthTTW `
:TT` a
$numTTb e
,TTe f
nullableTTg o
:TTo p
trueTTq u
)TTu v
,TTv w

Name_ValueUU 
=UU  
tableUU! &
.UU& '
ColumnUU' -
<UU- .
stringUU. 4
>UU4 5
(UU5 6
typeUU6 :
:UU: ;
$strUU< T
,UUT U
	maxLengthUUV _
:UU_ `
$numUUa d
,UUd e
nullableUUf n
:UUn o
falseUUp u
)UUu v
,UUv w
StockQuantityVV !
=VV" #
tableVV$ )
.VV) *
ColumnVV* 0
<VV0 1
intVV1 4
>VV4 5
(VV5 6
typeVV6 :
:VV: ;
$strVV< E
,VVE F
nullableVVG O
:VVO P
falseVVQ V
)VVV W
,VVW X
	CreatedAtWW 
=WW 
tableWW  %
.WW% &
ColumnWW& ,
<WW, -
DateTimeWW- 5
>WW5 6
(WW6 7
typeWW7 ;
:WW; <
$strWW= W
,WWW X
nullableWWY a
:WWa b
falseWWc h
)WWh i
,WWi j
	UpdatedAtXX 
=XX 
tableXX  %
.XX% &
ColumnXX& ,
<XX, -
DateTimeXX- 5
>XX5 6
(XX6 7
typeXX7 ;
:XX; <
$strXX= W
,XXW X
nullableXXY a
:XXa b
trueXXc g
)XXg h
,XXh i
	CreatedByYY 
=YY 
tableYY  %
.YY% &
ColumnYY& ,
<YY, -
stringYY- 3
>YY3 4
(YY4 5
typeYY5 9
:YY9 :
$strYY; A
,YYA B
nullableYYC K
:YYK L
trueYYM Q
)YYQ R
,YYR S
	UpdatedByZZ 
=ZZ 
tableZZ  %
.ZZ% &
ColumnZZ& ,
<ZZ, -
stringZZ- 3
>ZZ3 4
(ZZ4 5
typeZZ5 9
:ZZ9 :
$strZZ; A
,ZZA B
nullableZZC K
:ZZK L
trueZZM Q
)ZZQ R
,ZZR S
	IsDeleted[[ 
=[[ 
table[[  %
.[[% &
Column[[& ,
<[[, -
bool[[- 1
>[[1 2
([[2 3
type[[3 7
:[[7 8
$str[[9 B
,[[B C
nullable[[D L
:[[L M
false[[N S
)[[S T
}\\ 
,\\ 
constraints]] 
:]] 
table]] "
=>]]# %
{^^ 
table__ 
.__ 

PrimaryKey__ $
(__$ %
$str__% 2
,__2 3
x__4 5
=>__6 8
x__9 :
.__: ;
Id__; =
)__= >
;__> ?
table`` 
.`` 

ForeignKey`` $
(``$ %
nameaa 
:aa 
$straa A
,aaA B
columnbb 
:bb 
xbb  !
=>bb" $
xbb% &
.bb& '

CategoryIdbb' 1
,bb1 2
principalTablecc &
:cc& '
$strcc( 4
,cc4 5
principalColumndd '
:dd' (
$strdd) -
,dd- .
onDeleteee  
:ee  !
ReferentialActionee" 3
.ee3 4
Cascadeee4 ;
)ee; <
;ee< =
tableff 
.ff 

ForeignKeyff $
(ff$ %
namegg 
:gg 
$strgg B
,ggB C
columnhh 
:hh 
xhh  !
=>hh" $
xhh% &
.hh& '
CategoryId1hh' 2
,hh2 3
principalTableii &
:ii& '
$strii( 4
,ii4 5
principalColumnjj '
:jj' (
$strjj) -
)jj- .
;jj. /
}kk 
)kk 
;kk 
migrationBuildermm 
.mm 
CreateTablemm (
(mm( )
namenn 
:nn 
$strnn '
,nn' (
columnsoo 
:oo 
tableoo 
=>oo !
newoo" %
{pp 
Idqq 
=qq 
tableqq 
.qq 
Columnqq %
<qq% &
intqq& )
>qq) *
(qq* +
typeqq+ /
:qq/ 0
$strqq1 :
,qq: ;
nullableqq< D
:qqD E
falseqqF K
)qqK L
.rr 

Annotationrr #
(rr# $
$strrr$ D
,rrD E)
NpgsqlValueGenerationStrategyrrF c
.rrc d#
IdentityByDefaultColumnrrd {
)rr{ |
,rr| }
RoleIdss 
=ss 
tabless "
.ss" #
Columnss# )
<ss) *
intss* -
>ss- .
(ss. /
typess/ 3
:ss3 4
$strss5 >
,ss> ?
nullabless@ H
:ssH I
falsessJ O
)ssO P
,ssP Q
PermissionIdtt  
=tt! "
tablett# (
.tt( )
Columntt) /
<tt/ 0
inttt0 3
>tt3 4
(tt4 5
typett5 9
:tt9 :
$strtt; D
,ttD E
nullablettF N
:ttN O
falsettP U
)ttU V
,ttV W
	CreatedAtuu 
=uu 
tableuu  %
.uu% &
Columnuu& ,
<uu, -
DateTimeuu- 5
>uu5 6
(uu6 7
typeuu7 ;
:uu; <
$struu= W
,uuW X
nullableuuY a
:uua b
falseuuc h
)uuh i
,uui j
	UpdatedAtvv 
=vv 
tablevv  %
.vv% &
Columnvv& ,
<vv, -
DateTimevv- 5
>vv5 6
(vv6 7
typevv7 ;
:vv; <
$strvv= W
,vvW X
nullablevvY a
:vva b
truevvc g
)vvg h
,vvh i
	CreatedByww 
=ww 
tableww  %
.ww% &
Columnww& ,
<ww, -
stringww- 3
>ww3 4
(ww4 5
typeww5 9
:ww9 :
$strww; A
,wwA B
nullablewwC K
:wwK L
truewwM Q
)wwQ R
,wwR S
	UpdatedByxx 
=xx 
tablexx  %
.xx% &
Columnxx& ,
<xx, -
stringxx- 3
>xx3 4
(xx4 5
typexx5 9
:xx9 :
$strxx; A
,xxA B
nullablexxC K
:xxK L
truexxM Q
)xxQ R
,xxR S
	IsDeletedyy 
=yy 
tableyy  %
.yy% &
Columnyy& ,
<yy, -
boolyy- 1
>yy1 2
(yy2 3
typeyy3 7
:yy7 8
$stryy9 B
,yyB C
nullableyyD L
:yyL M
falseyyN S
)yyS T
}zz 
,zz 
constraints{{ 
:{{ 
table{{ "
=>{{# %
{|| 
table}} 
.}} 

PrimaryKey}} $
(}}$ %
$str}}% 9
,}}9 :
x}}; <
=>}}= ?
x}}@ A
.}}A B
Id}}B D
)}}D E
;}}E F
table~~ 
.~~ 

ForeignKey~~ $
(~~$ %
name 
: 
$str K
,K L
column
�� 
:
�� 
x
��  !
=>
��" $
x
��% &
.
��& '
PermissionId
��' 3
,
��3 4
principalTable
�� &
:
��& '
$str
��( 5
,
��5 6
principalColumn
�� '
:
��' (
$str
��) -
,
��- .
onDelete
��  
:
��  !
ReferentialAction
��" 3
.
��3 4
Cascade
��4 ;
)
��; <
;
��< =
table
�� 
.
�� 

ForeignKey
�� $
(
��$ %
name
�� 
:
�� 
$str
�� ?
,
��? @
column
�� 
:
�� 
x
��  !
=>
��" $
x
��% &
.
��& '
RoleId
��' -
,
��- .
principalTable
�� &
:
��& '
$str
��( /
,
��/ 0
principalColumn
�� '
:
��' (
$str
��) -
,
��- .
onDelete
��  
:
��  !
ReferentialAction
��" 3
.
��3 4
Cascade
��4 ;
)
��; <
;
��< =
}
�� 
)
�� 
;
�� 
migrationBuilder
�� 
.
�� 
CreateTable
�� (
(
��( )
name
�� 
:
�� 
$str
�� 
,
�� 
columns
�� 
:
�� 
table
�� 
=>
�� !
new
��" %
{
�� 
Id
�� 
=
�� 
table
�� 
.
�� 
Column
�� %
<
��% &
int
��& )
>
��) *
(
��* +
type
��+ /
:
��/ 0
$str
��1 :
,
��: ;
nullable
��< D
:
��D E
false
��F K
)
��K L
.
�� 

Annotation
�� #
(
��# $
$str
��$ D
,
��D E+
NpgsqlValueGenerationStrategy
��F c
.
��c d%
IdentityByDefaultColumn
��d {
)
��{ |
,
��| }
Adi
�� 
=
�� 
table
�� 
.
��  
Column
��  &
<
��& '
string
��' -
>
��- .
(
��. /
type
��/ 3
:
��3 4
$str
��5 L
,
��L M
	maxLength
��N W
:
��W X
$num
��Y [
,
��[ \
nullable
��] e
:
��e f
false
��g l
)
��l m
,
��m n
Soyadi
�� 
=
�� 
table
�� "
.
��" #
Column
��# )
<
��) *
string
��* 0
>
��0 1
(
��1 2
type
��2 6
:
��6 7
$str
��8 O
,
��O P
	maxLength
��Q Z
:
��Z [
$num
��\ ^
,
��^ _
nullable
��` h
:
��h i
false
��j o
)
��o p
,
��p q
PasswordHash
��  
=
��! "
table
��# (
.
��( )
Column
��) /
<
��/ 0
string
��0 6
>
��6 7
(
��7 8
type
��8 <
:
��< =
$str
��> D
,
��D E
nullable
��F N
:
��N O
false
��P U
)
��U V
,
��V W
IsAdmin
�� 
=
�� 
table
�� #
.
��# $
Column
��$ *
<
��* +
bool
��+ /
>
��/ 0
(
��0 1
type
��1 5
:
��5 6
$str
��7 @
,
��@ A
nullable
��B J
:
��J K
false
��L Q
,
��Q R
defaultValue
��S _
:
��_ `
false
��a f
)
��f g
,
��g h
LastLoginAt
�� 
=
��  !
table
��" '
.
��' (
Column
��( .
<
��. /
DateTime
��/ 7
>
��7 8
(
��8 9
type
��9 =
:
��= >
$str
��? Y
,
��Y Z
nullable
��[ c
:
��c d
true
��e i
)
��i j
,
��j k
Sicil
�� 
=
�� 
table
�� !
.
��! "
Column
��" (
<
��( )
string
��) /
>
��/ 0
(
��0 1
type
��1 5
:
��5 6
$str
��7 N
,
��N O
	maxLength
��P Y
:
��Y Z
$num
��[ ]
,
��] ^
nullable
��_ g
:
��g h
false
��i n
)
��n o
,
��o p
RoleId
�� 
=
�� 
table
�� "
.
��" #
Column
��# )
<
��) *
int
��* -
>
��- .
(
��. /
type
��/ 3
:
��3 4
$str
��5 >
,
��> ?
nullable
��@ H
:
��H I
true
��J N
)
��N O
,
��O P
	CreatedAt
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
DateTime
��- 5
>
��5 6
(
��6 7
type
��7 ;
:
��; <
$str
��= W
,
��W X
nullable
��Y a
:
��a b
false
��c h
)
��h i
,
��i j
	UpdatedAt
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
DateTime
��- 5
>
��5 6
(
��6 7
type
��7 ;
:
��; <
$str
��= W
,
��W X
nullable
��Y a
:
��a b
true
��c g
)
��g h
,
��h i
	CreatedBy
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
string
��- 3
>
��3 4
(
��4 5
type
��5 9
:
��9 :
$str
��; R
,
��R S
	maxLength
��T ]
:
��] ^
$num
��_ a
,
��a b
nullable
��c k
:
��k l
true
��m q
)
��q r
,
��r s
	UpdatedBy
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
string
��- 3
>
��3 4
(
��4 5
type
��5 9
:
��9 :
$str
��; R
,
��R S
	maxLength
��T ]
:
��] ^
$num
��_ a
,
��a b
nullable
��c k
:
��k l
true
��m q
)
��q r
,
��r s
	IsDeleted
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
bool
��- 1
>
��1 2
(
��2 3
type
��3 7
:
��7 8
$str
��9 B
,
��B C
nullable
��D L
:
��L M
false
��N S
,
��S T
defaultValue
��U a
:
��a b
false
��c h
)
��h i
}
�� 
,
�� 
constraints
�� 
:
�� 
table
�� "
=>
��# %
{
�� 
table
�� 
.
�� 

PrimaryKey
�� $
(
��$ %
$str
��% /
,
��/ 0
x
��1 2
=>
��3 5
x
��6 7
.
��7 8
Id
��8 :
)
��: ;
;
��; <
table
�� 
.
�� 

ForeignKey
�� $
(
��$ %
name
�� 
:
�� 
$str
�� 5
,
��5 6
column
�� 
:
�� 
x
��  !
=>
��" $
x
��% &
.
��& '
RoleId
��' -
,
��- .
principalTable
�� &
:
��& '
$str
��( /
,
��/ 0
principalColumn
�� '
:
��' (
$str
��) -
,
��- .
onDelete
��  
:
��  !
ReferentialAction
��" 3
.
��3 4
SetNull
��4 ;
)
��; <
;
��< =
}
�� 
)
�� 
;
�� 
migrationBuilder
�� 
.
�� 
CreateTable
�� (
(
��( )
name
�� 
:
�� 
$str
�� $
,
��$ %
columns
�� 
:
�� 
table
�� 
=>
�� !
new
��" %
{
�� 
Id
�� 
=
�� 
table
�� 
.
�� 
Column
�� %
<
��% &
int
��& )
>
��) *
(
��* +
type
��+ /
:
��/ 0
$str
��1 :
,
��: ;
nullable
��< D
:
��D E
false
��F K
)
��K L
.
�� 

Annotation
�� #
(
��# $
$str
��$ D
,
��D E+
NpgsqlValueGenerationStrategy
��F c
.
��c d%
IdentityByDefaultColumn
��d {
)
��{ |
,
��| }
UserId
�� 
=
�� 
table
�� "
.
��" #
Column
��# )
<
��) *
int
��* -
>
��- .
(
��. /
type
��/ 3
:
��3 4
$str
��5 >
,
��> ?
nullable
��@ H
:
��H I
true
��J N
)
��N O
,
��O P
Username
�� 
=
�� 
table
�� $
.
��$ %
Column
��% +
<
��+ ,
string
��, 2
>
��2 3
(
��3 4
type
��4 8
:
��8 9
$str
��: Q
,
��Q R
	maxLength
��S \
:
��\ ]
$num
��^ `
,
��` a
nullable
��b j
:
��j k
false
��l q
)
��q r
,
��r s
ActivityType
��  
=
��! "
table
��# (
.
��( )
Column
��) /
<
��/ 0
string
��0 6
>
��6 7
(
��7 8
type
��8 <
:
��< =
$str
��> U
,
��U V
	maxLength
��W `
:
��` a
$num
��b d
,
��d e
nullable
��f n
:
��n o
false
��p u
)
��u v
,
��v w
Description
�� 
=
��  !
table
��" '
.
��' (
Column
��( .
<
��. /
string
��/ 5
>
��5 6
(
��6 7
type
��7 ;
:
��; <
$str
��= U
,
��U V
	maxLength
��W `
:
��` a
$num
��b e
,
��e f
nullable
��g o
:
��o p
false
��q v
)
��v w
,
��w x
	Timestamp
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
DateTime
��- 5
>
��5 6
(
��6 7
type
��7 ;
:
��; <
$str
��= W
,
��W X
nullable
��Y a
:
��a b
false
��c h
)
��h i
,
��i j
AdditionalInfo
�� "
=
��# $
table
��% *
.
��* +
Column
��+ 1
<
��1 2
string
��2 8
>
��8 9
(
��9 :
type
��: >
:
��> ?
$str
��@ F
,
��F G
nullable
��H P
:
��P Q
true
��R V
)
��V W
,
��W X
IsSynchronized
�� "
=
��# $
table
��% *
.
��* +
Column
��+ 1
<
��1 2
bool
��2 6
>
��6 7
(
��7 8
type
��8 <
:
��< =
$str
��> G
,
��G H
nullable
��I Q
:
��Q R
false
��S X
,
��X Y
defaultValue
��Z f
:
��f g
false
��h m
)
��m n
,
��n o
	CreatedAt
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
DateTime
��- 5
>
��5 6
(
��6 7
type
��7 ;
:
��; <
$str
��= W
,
��W X
nullable
��Y a
:
��a b
false
��c h
)
��h i
,
��i j
	UpdatedAt
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
DateTime
��- 5
>
��5 6
(
��6 7
type
��7 ;
:
��; <
$str
��= W
,
��W X
nullable
��Y a
:
��a b
true
��c g
)
��g h
,
��h i
	CreatedBy
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
string
��- 3
>
��3 4
(
��4 5
type
��5 9
:
��9 :
$str
��; A
,
��A B
nullable
��C K
:
��K L
true
��M Q
)
��Q R
,
��R S
	UpdatedBy
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
string
��- 3
>
��3 4
(
��4 5
type
��5 9
:
��9 :
$str
��; A
,
��A B
nullable
��C K
:
��K L
true
��M Q
)
��Q R
,
��R S
	IsDeleted
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
bool
��- 1
>
��1 2
(
��2 3
type
��3 7
:
��7 8
$str
��9 B
,
��B C
nullable
��D L
:
��L M
false
��N S
,
��S T
defaultValue
��U a
:
��a b
false
��c h
)
��h i
}
�� 
,
�� 
constraints
�� 
:
�� 
table
�� "
=>
��# %
{
�� 
table
�� 
.
�� 

PrimaryKey
�� $
(
��$ %
$str
��% 6
,
��6 7
x
��8 9
=>
��: <
x
��= >
.
��> ?
Id
��? A
)
��A B
;
��B C
table
�� 
.
�� 

ForeignKey
�� $
(
��$ %
name
�� 
:
�� 
$str
�� <
,
��< =
column
�� 
:
�� 
x
��  !
=>
��" $
x
��% &
.
��& '
UserId
��' -
,
��- .
principalTable
�� &
:
��& '
$str
��( /
,
��/ 0
principalColumn
�� '
:
��' (
$str
��) -
,
��- .
onDelete
��  
:
��  !
ReferentialAction
��" 3
.
��3 4
SetNull
��4 ;
)
��; <
;
��< =
}
�� 
)
�� 
;
�� 
migrationBuilder
�� 
.
�� 
CreateTable
�� (
(
��( )
name
�� 
:
�� 
$str
�� !
,
��! "
columns
�� 
:
�� 
table
�� 
=>
�� !
new
��" %
{
�� 
Id
�� 
=
�� 
table
�� 
.
�� 
Column
�� %
<
��% &
int
��& )
>
��) *
(
��* +
type
��+ /
:
��/ 0
$str
��1 :
,
��: ;
nullable
��< D
:
��D E
false
��F K
)
��K L
.
�� 

Annotation
�� #
(
��# $
$str
��$ D
,
��D E+
NpgsqlValueGenerationStrategy
��F c
.
��c d%
IdentityByDefaultColumn
��d {
)
��{ |
,
��| }
Action
�� 
=
�� 
table
�� "
.
��" #
Column
��# )
<
��) *
string
��* 0
>
��0 1
(
��1 2
type
��2 6
:
��6 7
$str
��8 O
,
��O P
	maxLength
��Q Z
:
��Z [
$num
��\ ^
,
��^ _
nullable
��` h
:
��h i
false
��j o
)
��o p
,
��p q

EntityType
�� 
=
��  
table
��! &
.
��& '
Column
��' -
<
��- .
string
��. 4
>
��4 5
(
��5 6
type
��6 :
:
��: ;
$str
��< T
,
��T U
	maxLength
��V _
:
��_ `
$num
��a d
,
��d e
nullable
��f n
:
��n o
false
��p u
)
��u v
,
��v w
EntityId
�� 
=
�� 
table
�� $
.
��$ %
Column
��% +
<
��+ ,
string
��, 2
>
��2 3
(
��3 4
type
��4 8
:
��8 9
$str
��: Q
,
��Q R
	maxLength
��S \
:
��\ ]
$num
��^ `
,
��` a
nullable
��b j
:
��j k
false
��l q
)
��q r
,
��r s
UserId
�� 
=
�� 
table
�� "
.
��" #
Column
��# )
<
��) *
int
��* -
>
��- .
(
��. /
type
��/ 3
:
��3 4
$str
��5 >
,
��> ?
nullable
��@ H
:
��H I
true
��J N
)
��N O
,
��O P
Path
�� 
=
�� 
table
��  
.
��  !
Column
��! '
<
��' (
string
��( .
>
��. /
(
��/ 0
type
��0 4
:
��4 5
$str
��6 N
,
��N O
	maxLength
��P Y
:
��Y Z
$num
��[ ^
,
��^ _
nullable
��` h
:
��h i
false
��j o
)
��o p
,
��p q
Details
�� 
=
�� 
table
�� #
.
��# $
Column
��$ *
<
��* +
string
��+ 1
>
��1 2
(
��2 3
type
��3 7
:
��7 8
$str
��9 ?
,
��? @
nullable
��A I
:
��I J
false
��K P
)
��P Q
,
��Q R
	CreatedAt
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
DateTime
��- 5
>
��5 6
(
��6 7
type
��7 ;
:
��; <
$str
��= W
,
��W X
nullable
��Y a
:
��a b
false
��c h
)
��h i
,
��i j
	UpdatedAt
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
DateTime
��- 5
>
��5 6
(
��6 7
type
��7 ;
:
��; <
$str
��= W
,
��W X
nullable
��Y a
:
��a b
true
��c g
)
��g h
,
��h i
	CreatedBy
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
string
��- 3
>
��3 4
(
��4 5
type
��5 9
:
��9 :
$str
��; A
,
��A B
nullable
��C K
:
��K L
true
��M Q
)
��Q R
,
��R S
	UpdatedBy
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
string
��- 3
>
��3 4
(
��4 5
type
��5 9
:
��9 :
$str
��; A
,
��A B
nullable
��C K
:
��K L
true
��M Q
)
��Q R
,
��R S
	IsDeleted
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
bool
��- 1
>
��1 2
(
��2 3
type
��3 7
:
��7 8
$str
��9 B
,
��B C
nullable
��D L
:
��L M
false
��N S
,
��S T
defaultValue
��U a
:
��a b
false
��c h
)
��h i
}
�� 
,
�� 
constraints
�� 
:
�� 
table
�� "
=>
��# %
{
�� 
table
�� 
.
�� 

PrimaryKey
�� $
(
��$ %
$str
��% 3
,
��3 4
x
��5 6
=>
��7 9
x
��: ;
.
��; <
Id
��< >
)
��> ?
;
��? @
table
�� 
.
�� 

ForeignKey
�� $
(
��$ %
name
�� 
:
�� 
$str
�� 9
,
��9 :
column
�� 
:
�� 
x
��  !
=>
��" $
x
��% &
.
��& '
UserId
��' -
,
��- .
principalTable
�� &
:
��& '
$str
��( /
,
��/ 0
principalColumn
�� '
:
��' (
$str
��) -
,
��- .
onDelete
��  
:
��  !
ReferentialAction
��" 3
.
��3 4
SetNull
��4 ;
)
��; <
;
��< =
}
�� 
)
�� 
;
�� 
migrationBuilder
�� 
.
�� 
CreateTable
�� (
(
��( )
name
�� 
:
�� 
$str
�� '
,
��' (
columns
�� 
:
�� 
table
�� 
=>
�� !
new
��" %
{
�� 
Id
�� 
=
�� 
table
�� 
.
�� 
Column
�� %
<
��% &
int
��& )
>
��) *
(
��* +
type
��+ /
:
��/ 0
$str
��1 :
,
��: ;
nullable
��< D
:
��D E
false
��F K
)
��K L
.
�� 

Annotation
�� #
(
��# $
$str
��$ D
,
��D E+
NpgsqlValueGenerationStrategy
��F c
.
��c d%
IdentityByDefaultColumn
��d {
)
��{ |
,
��| }
UserId
�� 
=
�� 
table
�� "
.
��" #
Column
��# )
<
��) *
int
��* -
>
��- .
(
��. /
type
��/ 3
:
��3 4
$str
��5 >
,
��> ?
nullable
��@ H
:
��H I
false
��J O
)
��O P
,
��P Q
PermissionId
��  
=
��! "
table
��# (
.
��( )
Column
��) /
<
��/ 0
int
��0 3
>
��3 4
(
��4 5
type
��5 9
:
��9 :
$str
��; D
,
��D E
nullable
��F N
:
��N O
false
��P U
)
��U V
,
��V W
	IsGranted
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
bool
��- 1
>
��1 2
(
��2 3
type
��3 7
:
��7 8
$str
��9 B
,
��B C
nullable
��D L
:
��L M
false
��N S
,
��S T
defaultValue
��U a
:
��a b
true
��c g
)
��g h
,
��h i
	CreatedAt
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
DateTime
��- 5
>
��5 6
(
��6 7
type
��7 ;
:
��; <
$str
��= W
,
��W X
nullable
��Y a
:
��a b
false
��c h
)
��h i
,
��i j
	UpdatedAt
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
DateTime
��- 5
>
��5 6
(
��6 7
type
��7 ;
:
��; <
$str
��= W
,
��W X
nullable
��Y a
:
��a b
true
��c g
)
��g h
,
��h i
	CreatedBy
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
string
��- 3
>
��3 4
(
��4 5
type
��5 9
:
��9 :
$str
��; A
,
��A B
nullable
��C K
:
��K L
true
��M Q
)
��Q R
,
��R S
	UpdatedBy
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
string
��- 3
>
��3 4
(
��4 5
type
��5 9
:
��9 :
$str
��; A
,
��A B
nullable
��C K
:
��K L
true
��M Q
)
��Q R
,
��R S
	IsDeleted
�� 
=
�� 
table
��  %
.
��% &
Column
��& ,
<
��, -
bool
��- 1
>
��1 2
(
��2 3
type
��3 7
:
��7 8
$str
��9 B
,
��B C
nullable
��D L
:
��L M
false
��N S
)
��S T
}
�� 
,
�� 
constraints
�� 
:
�� 
table
�� "
=>
��# %
{
�� 
table
�� 
.
�� 

PrimaryKey
�� $
(
��$ %
$str
��% 9
,
��9 :
x
��; <
=>
��= ?
x
��@ A
.
��A B
Id
��B D
)
��D E
;
��E F
table
�� 
.
�� 

ForeignKey
�� $
(
��$ %
name
�� 
:
�� 
$str
�� K
,
��K L
column
�� 
:
�� 
x
��  !
=>
��" $
x
��% &
.
��& '
PermissionId
��' 3
,
��3 4
principalTable
�� &
:
��& '
$str
��( 5
,
��5 6
principalColumn
�� '
:
��' (
$str
��) -
,
��- .
onDelete
��  
:
��  !
ReferentialAction
��" 3
.
��3 4
Cascade
��4 ;
)
��; <
;
��< =
table
�� 
.
�� 

ForeignKey
�� $
(
��$ %
name
�� 
:
�� 
$str
�� ?
,
��? @
column
�� 
:
�� 
x
��  !
=>
��" $
x
��% &
.
��& '
UserId
��' -
,
��- .
principalTable
�� &
:
��& '
$str
��( /
,
��/ 0
principalColumn
�� '
:
��' (
$str
��) -
,
��- .
onDelete
��  
:
��  !
ReferentialAction
��" 3
.
��3 4
Cascade
��4 ;
)
��; <
;
��< =
}
�� 
)
�� 
;
�� 
migrationBuilder
�� 
.
�� 

InsertData
�� '
(
��' (
table
�� 
:
�� 
$str
�� 
,
�� 
columns
�� 
:
�� 
new
�� 
[
�� 
]
�� 
{
��  
$str
��! %
,
��% &
$str
��' 2
,
��2 3
$str
��4 ?
,
��? @
$str
��A N
,
��N O
$str
��P V
,
��V W
$str
��X c
,
��c d
$str
��e p
}
��q r
,
��r s
values
�� 
:
�� 
new
�� 
object
�� "
[
��" #
]
��# $
{
��% &
$num
��' (
,
��( )
new
��* -
DateTime
��. 6
(
��6 7
$num
��7 ;
,
��; <
$num
��= >
,
��> ?
$num
��@ A
,
��A B
$num
��C D
,
��D E
$num
��F G
,
��G H
$num
��I J
,
��J K
$num
��L M
,
��M N
DateTimeKind
��O [
.
��[ \
Utc
��\ _
)
��_ `
,
��` a
null
��b f
,
��f g
$str��h �
,��� �
$str��� �
,��� �
null��� �
,��� �
null��� �
}��� �
)��� �
;��� �
migrationBuilder
�� 
.
�� 

InsertData
�� '
(
��' (
table
�� 
:
�� 
$str
�� 
,
�� 
columns
�� 
:
�� 
new
�� 
[
�� 
]
�� 
{
��  
$str
��! %
,
��% &
$str
��' ,
,
��, -
$str
��. 9
,
��9 :
$str
��; F
,
��F G
$str
��H Q
,
��Q R
$str
��S `
,
��` a
$str
��b p
,
��p q
$str
��r z
,
��z {
$str��| �
,��� �
$str��� �
,��� �
$str��� �
,��� �
$str��� �
}��� �
,��� �
values
�� 
:
�� 
new
�� 
object
�� "
[
��" #
]
��# $
{
��% &
$num
��' (
,
��( )
$str
��* 1
,
��1 2
new
��3 6
DateTime
��7 ?
(
��? @
$num
��@ D
,
��D E
$num
��F G
,
��G H
$num
��I J
,
��J K
$num
��L M
,
��M N
$num
��O P
,
��P Q
$num
��R S
,
��S T
$num
��U V
,
��V W
DateTimeKind
��X d
.
��d e
Utc
��e h
)
��h i
,
��i j
null
��k o
,
��o p
true
��q u
,
��u v
null
��w {
,
��{ |
$str��} �
,��� �
$num��� �
,��� �
$str��� �
,��� �
$str��� �
,��� �
null��� �
,��� �
null��� �
}��� �
)��� �
;��� �
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� .
,
��. /
table
�� 
:
�� 
$str
�� %
,
��% &
column
�� 
:
�� 
$str
��  
)
��  !
;
��! "
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� +
,
��+ ,
table
�� 
:
�� 
$str
�� "
,
��" #
column
�� 
:
�� 
$str
��  
)
��  !
;
��! "
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� *
,
��* +
table
�� 
:
�� 
$str
�� #
,
��# $
column
�� 
:
�� 
$str
�� 
,
�� 
unique
�� 
:
�� 
true
�� 
)
�� 
;
�� 
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� +
,
��+ ,
table
�� 
:
�� 
$str
�� $
,
��$ %
column
�� 
:
�� 
$str
�� 
,
�� 
unique
�� 
:
�� 
true
�� 
,
�� 
filter
�� 
:
�� 
$str
�� /
)
��/ 0
;
��0 1
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� .
,
��. /
table
�� 
:
�� 
$str
�� !
,
��! "
column
�� 
:
�� 
$str
�� $
)
��$ %
;
��% &
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� /
,
��/ 0
table
�� 
:
�� 
$str
�� !
,
��! "
column
�� 
:
�� 
$str
�� %
)
��% &
;
��& '
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� 7
,
��7 8
table
�� 
:
�� 
$str
�� (
,
��( )
column
�� 
:
�� 
$str
�� &
)
��& '
;
��' (
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� >
,
��> ?
table
�� 
:
�� 
$str
�� (
,
��( )
columns
�� 
:
�� 
new
�� 
[
�� 
]
�� 
{
��  
$str
��! )
,
��) *
$str
��+ 9
}
��: ;
,
��; <
unique
�� 
:
�� 
true
�� 
)
�� 
;
�� 
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� %
,
��% &
table
�� 
:
�� 
$str
�� 
,
�� 
column
�� 
:
�� 
$str
�� 
,
�� 
unique
�� 
:
�� 
true
�� 
,
�� 
filter
�� 
:
�� 
$str
�� /
)
��/ 0
;
��0 1
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� 7
,
��7 8
table
�� 
:
�� 
$str
�� (
,
��( )
column
�� 
:
�� 
$str
�� &
)
��& '
;
��' (
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� >
,
��> ?
table
�� 
:
�� 
$str
�� (
,
��( )
columns
�� 
:
�� 
new
�� 
[
�� 
]
�� 
{
��  
$str
��! )
,
��) *
$str
��+ 9
}
��: ;
,
��; <
unique
�� 
:
�� 
true
�� 
)
�� 
;
�� 
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� '
,
��' (
table
�� 
:
�� 
$str
�� 
,
�� 
column
�� 
:
�� 
$str
��  
)
��  !
;
��! "
migrationBuilder
�� 
.
�� 
CreateIndex
�� (
(
��( )
name
�� 
:
�� 
$str
�� &
,
��& '
table
�� 
:
�� 
$str
�� 
,
�� 
column
�� 
:
�� 
$str
�� 
,
��  
unique
�� 
:
�� 
true
�� 
,
�� 
filter
�� 
:
�� 
$str
�� /
)
��/ 0
;
��0 1
}
�� 	
	protected
�� 
override
�� 
void
�� 
Down
��  $
(
��$ %
MigrationBuilder
��% 5
migrationBuilder
��6 F
)
��F G
{
�� 	
migrationBuilder
�� 
.
�� 
	DropTable
�� &
(
��& '
name
�� 
:
�� 
$str
�� $
)
��$ %
;
��% &
migrationBuilder
�� 
.
�� 
	DropTable
�� &
(
��& '
name
�� 
:
�� 
$str
�� !
)
��! "
;
��" #
migrationBuilder
�� 
.
�� 
	DropTable
�� &
(
��& '
name
�� 
:
�� 
$str
��  
)
��  !
;
��! "
migrationBuilder
�� 
.
�� 
	DropTable
�� &
(
��& '
name
�� 
:
�� 
$str
�� '
)
��' (
;
��( )
migrationBuilder
�� 
.
�� 
	DropTable
�� &
(
��& '
name
�� 
:
�� 
$str
�� '
)
��' (
;
��( )
migrationBuilder
�� 
.
�� 
	DropTable
�� &
(
��& '
name
�� 
:
�� 
$str
�� "
)
��" #
;
��# $
migrationBuilder
�� 
.
�� 
	DropTable
�� &
(
��& '
name
�� 
:
�� 
$str
�� #
)
��# $
;
��$ %
migrationBuilder
�� 
.
�� 
	DropTable
�� &
(
��& '
name
�� 
:
�� 
$str
�� 
)
�� 
;
�� 
migrationBuilder
�� 
.
�� 
	DropTable
�� &
(
��& '
name
�� 
:
�� 
$str
�� 
)
�� 
;
�� 
}
�� 	
}
�� 
}�� �
zC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Migrations\20250313001630_UpdateUserUniqueConstraints.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 

Migrations )
{ 
public 

partial 
class '
UpdateUserUniqueConstraints 4
:5 6
	Migration7 @
{		 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
CreateIndex (
(( )
name 
: 
$str &
,& '
table 
: 
$str 
, 
column 
: 
$str 
,  
unique 
: 
true 
) 
; 
} 	
	protected 
override 
void 
Down  $
($ %
MigrationBuilder% 5
migrationBuilder6 F
)F G
{ 	
migrationBuilder 
. 
	DropIndex &
(& '
name 
: 
$str &
,& '
table 
: 
$str 
) 
;  
} 	
} 
} �
ZC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Logging\LoggerManager.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Logging &
{ 
public 

class 
LoggerManager 
< 
T  
>  !
:" #
ILoggerManager$ 2
<2 3
T3 4
>4 5
where6 ;
T< =
:> ?
class@ E
{ 
private 
readonly 
ILogger  
<  !
T! "
>" #
_logger$ +
;+ ,
public 
LoggerManager 
( 
ILogger $
<$ %
T% &
>& '
logger( .
). /
{ 	
_logger 
= 
logger 
; 
} 	
public 
void 
LogInfo 
( 
string "
message# *
,* +
params, 2
object3 9
[9 :
]: ;
args< @
)@ A
{   	
_logger!! 
.!! 
LogInformation!! "
(!!" #
message!!# *
,!!* +
args!!, 0
)!!0 1
;!!1 2
}"" 	
public)) 
void)) 
LogWarn)) 
()) 
string)) "
message))# *
,))* +
params)), 2
object))3 9
[))9 :
])): ;
args))< @
)))@ A
{** 	
_logger++ 
.++ 

LogWarning++ 
(++ 
message++ &
,++& '
args++( ,
)++, -
;++- .
},, 	
public33 
void33 
LogDebug33 
(33 
string33 #
message33$ +
,33+ ,
params33- 3
object334 :
[33: ;
]33; <
args33= A
)33A B
{44 	
_logger55 
.55 
LogDebug55 
(55 
message55 $
,55$ %
args55& *
)55* +
;55+ ,
}66 	
public== 
void== 
LogError== 
(== 
string== #
message==$ +
,==+ ,
params==- 3
object==4 :
[==: ;
]==; <
args=== A
)==A B
{>> 	
_logger?? 
.?? 
LogError?? 
(?? 
message?? $
,??$ %
args??& *
)??* +
;??+ ,
}@@ 	
publicHH 
voidHH 
LogErrorHH 
(HH 
	ExceptionHH &
exHH' )
,HH) *
stringHH+ 1
messageHH2 9
,HH9 :
paramsHH; A
objectHHB H
[HHH I
]HHI J
argsHHK O
)HHO P
{II 	
_loggerJJ 
.JJ 
LogErrorJJ 
(JJ 
exJJ 
,JJ  
messageJJ! (
,JJ( )
argsJJ* .
)JJ. /
;JJ/ 0
}KK 	
}LL 
}MM �%
XC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\DependencyInjection.cs
	namespace 	
Stock
 
. 
Infrastructure 
{ 
public 

static 
class 
DependencyInjection +
{ 
public 
static 
IServiceCollection (
AddInfrastructure) :
(: ;
this; ?
IServiceCollection@ R
servicesS [
,[ \
IConfiguration] k
configurationl y
)y z
{ 	
services 
. 
AddDbContext !
<! " 
ApplicationDbContext" 6
>6 7
(7 8
options8 ?
=>@ B
options 
. 
	UseNpgsql !
(! "
configuration !
.! "
GetConnectionString" 5
(5 6
$str6 I
)I J
,J K
b 
=> 
b 
. 
MigrationsAssembly -
(- .
typeof. 4
(4 5 
ApplicationDbContext5 I
)I J
.J K
AssemblyK S
.S T
FullNameT \
)\ ]
)] ^
)^ _
;_ `
services 
. 
	AddScoped 
( 
typeof %
(% &
IRepository& 1
<1 2
>2 3
)3 4
,4 5
typeof6 <
(< =
GenericRepository= N
<N O
>O P
)P Q
)Q R
;R S
services 
. 
	AddScoped 
< 
IUserRepository .
,. /
UserRepository0 >
>> ?
(? @
)@ A
;A B
services 
. 
	AddScoped 
< 
IRoleRepository .
,. /
RoleRepository0 >
>> ?
(? @
)@ A
;A B
services 
. 
	AddScoped 
< !
IPermissionRepository 4
,4 5 
PermissionRepository6 J
>J K
(K L
)L M
;M N
services 
. 
	AddScoped 
< 
IProductRepository 1
,1 2
ProductRepository3 D
>D E
(E F
)F G
;G H
services 
. 
	AddScoped 
< 
ICategoryRepository 2
,2 3
CategoryRepository4 F
>F G
(G H
)H I
;I J
services!! 
.!! 
	AddScoped!! 
<!! 
IUnitOfWork!! *
,!!* +
Data!!, 0
.!!0 1

UnitOfWork!!1 ;
>!!; <
(!!< =
)!!= >
;!!> ?
services$$ 
.$$ 
	AddScoped$$ 
<$$ 
IPasswordHasher$$ .
,$$. /
PasswordHasher$$0 >
>$$> ?
($$? @
)$$@ A
;$$A B
services%% 
.%% 
	AddScoped%% 
<%% 
IJwtTokenGenerator%% 1
,%%1 2
JwtTokenGenerator%%3 D
>%%D E
(%%E F
)%%F G
;%%G H
services&& 
.&& 
	AddScoped&& 
<&& 
IAuthService&& +
,&&+ ,
AuthService&&- 8
>&&8 9
(&&9 :
)&&: ;
;&&; <
services'' 
.'' 
	AddScoped'' 
<'' 
IAuditService'' ,
,'', -
AuditService''. :
>'': ;
(''; <
)''< =
;''= >
services(( 
.(( 
	AddScoped(( 
<(( 
IPermissionService(( 1
,((1 2
PermissionService((3 D
>((D E
(((E F
)((F G
;((G H
services)) 
.)) 
	AddScoped)) 
<)) "
IUserPermissionService)) 5
,))5 6!
UserPermissionService))7 L
>))L M
())M N
)))N O
;))O P
services** 
.** 
	AddScoped** 
<** 
ICurrentUserService** 2
,**2 3
CurrentUserService**4 F
>**F G
(**G H
)**H I
;**I J
services-- 
.-- 
AddSingleton-- !
(--! "
typeof--" (
(--( )
ILoggerManager--) 7
<--7 8
>--8 9
)--9 :
,--: ;
typeof--< B
(--B C
LoggerManager--C P
<--P Q
>--Q R
)--R S
)--S T
;--T U
return// 
services// 
;// 
}00 	
}11 
}22 �/
TC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\UnitOfWork.cs
	namespace		 	
Stock		
 
.		 
Infrastructure		 
.		 
Data		 #
{

 
public 

class 

UnitOfWork 
: 
IUnitOfWork )
{ 
private 
readonly  
ApplicationDbContext -
_context. 6
;6 7
private 
IUserRepository 
?  
_users! '
;' (
private 
IRoleRepository 
?  
_roles! '
;' (
private !
IPermissionRepository %
?% &
_permissions' 3
;3 4
private 
IProductRepository "
?" #
	_products$ -
;- .
private 
ICategoryRepository #
?# $
_categories% 0
;0 1
private 
bool 
	_disposed 
; 
public 

UnitOfWork 
(  
ApplicationDbContext .
context/ 6
)6 7
{ 	
_context 
= 
context 
; 
} 	
public 
IUserRepository 
Users $
=>% '
_users( .
??=/ 2
new3 6
UserRepository7 E
(E F
_contextF N
)N O
;O P
public!! 
IRoleRepository!! 
Roles!! $
=>!!% '
_roles!!( .
??=!!/ 2
new!!3 6
RoleRepository!!7 E
(!!E F
_context!!F N
)!!N O
;!!O P
public## !
IPermissionRepository## $
Permissions##% 0
=>##1 3
_permissions##4 @
??=##A D
new##E H 
PermissionRepository##I ]
(##] ^
_context##^ f
)##f g
;##g h
public$$ 
IProductRepository$$ !
Products$$" *
=>$$+ -
	_products$$. 7
??=$$8 ;
new$$< ?
ProductRepository$$@ Q
($$Q R
_context$$R Z
)$$Z [
;$$[ \
public%% 
ICategoryRepository%% "

Categories%%# -
=>%%. 0
_categories%%1 <
??=%%= @
new%%A D
CategoryRepository%%E W
(%%W X
_context%%X `
)%%` a
;%%a b
public(( 
IRepository(( 
<(( 
T(( 
>(( 
GetRepository(( +
<((+ ,
T((, -
>((- .
(((. /
)((/ 0
where((1 6
T((7 8
:((9 :

BaseEntity((; E
{)) 	
return++ 
new++ 
GenericRepository++ (
<++( )
T++) *
>++* +
(+++ ,
_context++, 4
)++4 5
;++5 6
},, 	
public// 
async// 
Task// 
<// 
int// 
>// 
SaveChangesAsync// /
(/// 0
CancellationToken//0 A
cancellationToken//B S
=//T U
default//V ]
)//] ^
{00 	
return22 
await22 
_context22 !
.22! "
SaveChangesAsync22" 2
(222 3
cancellationToken223 D
)22D E
;22E F
}33 	
public66 
async66 
Task66 !
BeginTransactionAsync66 /
(66/ 0
CancellationToken660 A
cancellationToken66B S
=66T U
default66V ]
)66] ^
{77 	
await88 
_context88 
.88 
Database88 #
.88# $!
BeginTransactionAsync88$ 9
(889 :
cancellationToken88: K
)88K L
;88L M
}99 	
public<< 
async<< 
Task<< "
CommitTransactionAsync<< 0
(<<0 1
CancellationToken<<1 B
cancellationToken<<C T
=<<U V
default<<W ^
)<<^ _
{== 	
try>> 
{?? 
await@@ 
_context@@ 
.@@ 
SaveChangesAsync@@ /
(@@/ 0
cancellationToken@@0 A
)@@A B
;@@B C
awaitAA 
_contextAA 
.AA 
DatabaseAA '
.AA' ("
CommitTransactionAsyncAA( >
(AA> ?
cancellationTokenAA? P
)AAP Q
;AAQ R
}BB 
catchCC 
{DD 
awaitEE $
RollbackTransactionAsyncEE .
(EE. /
cancellationTokenEE/ @
)EE@ A
;EEA B
throwFF 
;FF 
}GG 
}HH 	
publicKK 
asyncKK 
TaskKK $
RollbackTransactionAsyncKK 2
(KK2 3
CancellationTokenKK3 D
cancellationTokenKKE V
=KKW X
defaultKKY `
)KK` a
{LL 	
awaitOO 
_contextOO 
.OO 
DatabaseOO #
.OO# $$
RollbackTransactionAsyncOO$ <
(OO< =
cancellationTokenOO= N
)OON O
;OOO P
}PP 	
publicSS 
voidSS 
DisposeSS 
(SS 
)SS 
{TT 	
DisposeUU 
(UU 
trueUU 
)UU 
;UU 
GCVV 
.VV 
SuppressFinalizeVV 
(VV  
thisVV  $
)VV$ %
;VV% &
}WW 	
	protectedYY 
virtualYY 
voidYY 
DisposeYY &
(YY& '
boolYY' +
	disposingYY, 5
)YY5 6
{ZZ 	
if[[ 
([[ 
![[ 
	_disposed[[ 
&&[[ 
	disposing[[ '
)[[' (
{\\ 
_context]] 
.]] 
Dispose]]  
(]]  !
)]]! "
;]]" #
}^^ 
	_disposed__ 
=__ 
true__ 
;__ 
}`` 	
}aa 
}bb �'
oC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Specifications\SpecificationEvaluator.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Specifications$ 2
{ 
public 

static 
class "
SpecificationEvaluator .
<. /
T/ 0
>0 1
where2 7
T8 9
:: ;

BaseEntity< F
{ 
public 
static 

IQueryable  
<  !
T! "
>" #
GetQuery$ ,
(, -

IQueryable- 7
<7 8
T8 9
>9 :

inputQuery; E
,E F
ISpecificationG U
<U V
TV W
>W X
specY ]
)] ^
{ 	
var 
query 
= 

inputQuery "
;" #
if 
( 
spec 
. 
Criteria 
!=  
null! %
)% &
{ 
query 
= 
query 
. 
Where #
(# $
spec$ (
.( )
Criteria) 1
)1 2
;2 3
} 
query 
= 
spec 
. 
Includes !
.! "
	Aggregate" +
(+ ,
query, 1
,1 2
(3 4
current4 ;
,; <
include= D
)D E
=>F H
currentI P
.P Q
IncludeQ X
(X Y
includeY `
)` a
)a b
;b c
query"" 
="" 
spec"" 
."" 
IncludeStrings"" '
.""' (
	Aggregate""( 1
(""1 2
query""2 7
,""7 8
(""9 :
current"": A
,""A B
include""C J
)""J K
=>""L N
current""O V
.""V W
Include""W ^
(""^ _
include""_ f
)""f g
)""g h
;""h i
bool%% 
	isOrdered%% 
=%% 
false%% "
;%%" #
if&& 
(&& 
spec&& 
.&& 
OrderBy&& 
!=&& 
null&&  $
)&&$ %
{'' 
query(( 
=(( 
query(( 
.(( 
OrderBy(( %
(((% &
spec((& *
.((* +
OrderBy((+ 2
)((2 3
;((3 4
	isOrdered)) 
=)) 
true))  
;))  !
}** 
else++ 
if++ 
(++ 
spec++ 
.++ 
OrderByDescending++ +
!=++, .
null++/ 3
)++3 4
{,, 
query-- 
=-- 
query-- 
.-- 
OrderByDescending-- /
(--/ 0
spec--0 4
.--4 5
OrderByDescending--5 F
)--F G
;--G H
	isOrdered.. 
=.. 
true..  
;..  !
}// 
if22 
(22 
	isOrdered22 
)22 
{33 
if44 
(44 
spec44 
.44 
ThenBy44 
!=44  "
null44# '
)44' (
{55 
query88 
=88 
(88 
(88 
IOrderedQueryable88 /
<88/ 0
T880 1
>881 2
)882 3
query883 8
)888 9
.889 :
ThenBy88: @
(88@ A
spec88A E
.88E F
ThenBy88F L
)88L M
;88M N
}99 
else:: 
if:: 
(:: 
spec:: 
.:: 
ThenByDescending:: .
!=::/ 1
null::2 6
)::6 7
{;; 
query<< 
=<< 
(<< 
(<< 
IOrderedQueryable<< /
<<</ 0
T<<0 1
><<1 2
)<<2 3
query<<3 8
)<<8 9
.<<9 :
ThenByDescending<<: J
(<<J K
spec<<K O
.<<O P
ThenByDescending<<P `
)<<` a
;<<a b
}== 
}>> 
ifAA 
(AA 
specAA 
.AA 
IsPagingEnabledAA $
)AA$ %
{BB 
queryCC 
=CC 
queryCC 
.CC 
SkipCC "
(CC" #
specCC# '
.CC' (
SkipCC( ,
)CC, -
.CC- .
TakeCC. 2
(CC2 3
specCC3 7
.CC7 8
TakeCC8 <
)CC< =
;CC= >
}DD 
ifGG 
(GG 
specGG 
.GG 
IsAsNoTrackingGG #
)GG# $
{HH 
queryII 
=II 
queryII 
.II 
AsNoTrackingII *
(II* +
)II+ ,
;II, -
}JJ 
returnSS 
querySS 
;SS 
}TT 	
}UU 
}VV �
eC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Repositories\UserRepository.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Repositories$ 0
{ 
public 

class 
UserRepository 
:  !
GenericRepository" 3
<3 4
User4 8
>8 9
,9 :
IUserRepository; J
{ 
public 
UserRepository 
(  
ApplicationDbContext 2
context3 :
): ;
:< =
base> B
(B C
contextC J
)J K
{ 	
} 	
public 
async 
Task 
< 
User 
? 
>  
GetBySicilAsync! 0
(0 1
string1 7
sicil8 =
)= >
{ 	
return 
await 
_dbSet 
. 
Where 
( 
u 
=> 
u 
. 
Sicil #
==$ &
sicil' ,
&&- /
!0 1
u1 2
.2 3
	IsDeleted3 <
)< =
. 
FirstOrDefaultAsync $
($ %
)% &
;& '
} 	
}   
}!! �
eC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Repositories\RoleRepository.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Repositories$ 0
{ 
public 

class 
RoleRepository 
:  !
GenericRepository" 3
<3 4
Role4 8
>8 9
,9 :
IRoleRepository; J
{ 
public 
RoleRepository 
(  
ApplicationDbContext 2
context3 :
): ;
:< =
base> B
(B C
contextC J
)J K
{ 	
} 	
} 
}   �
kC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Repositories\PermissionRepository.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Repositories$ 0
{ 
public 

class  
PermissionRepository %
:& '
GenericRepository( 9
<9 :

Permission: D
>D E
,E F!
IPermissionRepositoryG \
{ 
public  
PermissionRepository #
(# $ 
ApplicationDbContext$ 8
context9 @
)@ A
:B C
baseD H
(H I
contextI P
)P Q
{ 	
} 	
} 
} �%
hC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Repositories\ProductRepository.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Repositories$ 0
{ 
public 

class 
ProductRepository "
:# $
IProductRepository% 7
{		 
private

 
readonly

  
ApplicationDbContext

 -
_context

. 6
;

6 7
public 
ProductRepository  
(  ! 
ApplicationDbContext! 5
context6 =
)= >
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
Product !
>! "
GetByIdAsync# /
(/ 0
int0 3
id4 6
)6 7
{ 	
var 
product 
= 
await 
_context  (
.( )
Products) 1
. 
Include 
( 
p 
=> 
p 
.  
Category  (
)( )
. 
FirstOrDefaultAsync $
($ %
p% &
=>' )
p* +
.+ ,
Id, .
==/ 1
id2 4
&&5 7
!8 9
p9 :
.: ;
	IsDeleted; D
)D E
;E F
if 
( 
product 
== 
null 
)  
{ 
throw 
new  
KeyNotFoundException .
(. /
$"/ 1
$str1 A
{A B
idB D
}D E
$strE P
"P Q
)Q R
;R S
} 
return 
product 
; 
} 	
public   
async   
Task   
<   
IEnumerable   %
<  % &
Product  & -
>  - .
>  . /
GetAllAsync  0 ;
(  ; <
)  < =
{!! 	
return"" 
await"" 
_context"" !
.""! "
Products""" *
.## 
Include## 
(## 
p## 
=>## 
p## 
.##  
Category##  (
)##( )
.$$ 
Where$$ 
($$ 
p$$ 
=>$$ 
!$$ 
p$$ 
.$$ 
	IsDeleted$$ (
)$$( )
.%% 
ToListAsync%% 
(%% 
)%% 
;%% 
}&& 	
public(( 
async(( 
Task(( 
<(( 
Product(( !
>((! "
AddAsync((# +
(((+ ,
Product((, 3
product((4 ;
)((; <
{)) 	
await** 
_context** 
.** 
Products** #
.**# $
AddAsync**$ ,
(**, -
product**- 4
)**4 5
;**5 6
return,, 
product,, 
;,, 
}-- 	
public// 
Task// 
UpdateAsync// 
(//  
Product//  '
product//( /
)/// 0
{00 	
_context11 
.11 
Entry11 
(11 
product11 "
)11" #
.11# $
State11$ )
=11* +
EntityState11, 7
.117 8
Modified118 @
;11@ A
return33 
Task33 
.33 
CompletedTask33 %
;33% &
}44 	
public66 
Task66 
DeleteAsync66 
(66  
Product66  '
product66( /
)66/ 0
{77 	
product88 
.88 
	IsDeleted88 
=88 
true88  $
;88$ %
_context99 
.99 
Entry99 
(99 
product99 "
)99" #
.99# $
State99$ )
=99* +
EntityState99, 7
.997 8
Modified998 @
;99@ A
return;; 
Task;; 
.;; 
CompletedTask;; %
;;;% &
}<< 	
public>> 
Task>> 
<>> 
int>> 
>>> 
SaveChangesAsync>> )
(>>) *
CancellationToken>>* ;
cancellationToken>>< M
=>>N O
default>>P W
)>>W X
{?? 	
return@@ 
_context@@ 
.@@ 
SaveChangesAsync@@ ,
(@@, -
cancellationToken@@- >
)@@> ?
;@@? @
}AA 	
}LL 
}MM �[
hC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Repositories\GenericRepository.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Repositories$ 0
{ 
public 

class 
GenericRepository "
<" #
T# $
>$ %
:& '
IRepository( 3
<3 4
T4 5
>5 6
where7 <
T= >
:? @

BaseEntityA K
{ 
	protected 
readonly  
ApplicationDbContext /
_context0 8
;8 9
	protected 
readonly 
DbSet  
<  !
T! "
>" #
_dbSet$ *
;* +
public"" 
GenericRepository""  
(""  ! 
ApplicationDbContext""! 5
context""6 =
)""= >
{## 	
_context$$ 
=$$ 
context$$ 
;$$ 
_dbSet%% 
=%% 
context%% 
.%% 
Set%%  
<%%  !
T%%! "
>%%" #
(%%# $
)%%$ %
;%%% &
}&& 	
public)) 
virtual)) 
async)) 
Task)) !
<))! "
T))" #
?))# $
>))$ %
FirstOrDefaultAsync))& 9
())9 :
ISpecification)): H
<))H I
T))I J
>))J K
spec))L P
,))P Q
CancellationToken))R c
cancellationToken))d u
=))v w
default))x 
)	)) �
{** 	
return++ 
await++ 
ApplySpecification++ +
(+++ ,
spec++, 0
)++0 1
.++1 2
FirstOrDefaultAsync++2 E
(++E F
cancellationToken++F W
)++W X
;++X Y
},, 	
public// 
virtual// 
async// 
Task// !
<//! "
IReadOnlyList//" /
</// 0
T//0 1
>//1 2
>//2 3
	ListAsync//4 =
(//= >
ISpecification//> L
<//L M
T//M N
>//N O
spec//P T
,//T U
CancellationToken//V g
cancellationToken//h y
=//z {
default	//| �
)
//� �
{00 	
return11 
await11 
ApplySpecification11 +
(11+ ,
spec11, 0
)110 1
.111 2
ToListAsync112 =
(11= >
cancellationToken11> O
)11O P
;11P Q
}22 	
public55 
virtual55 
async55 
Task55 !
<55! "
T55" #
?55# $
>55$ %
GetByIdAsync55& 2
(552 3
int553 6
id557 9
,559 :
CancellationToken55; L
cancellationToken55M ^
=55_ `
default55a h
)55h i
{66 	
return88 
await88 
_dbSet88 
.88  
	FindAsync88  )
(88) *
new88* -
object88. 4
[884 5
]885 6
{887 8
id889 ;
}88< =
,88= >
cancellationToken88? P
:88P Q
cancellationToken88R c
)88c d
;88d e
}99 	
public<< 
virtual<< 
async<< 
Task<< !
<<<! "
IEnumerable<<" -
<<<- .
T<<. /
><</ 0
><<0 1
ListEnumerableAsync<<2 E
(<<E F
ISpecification<<F T
<<<T U
T<<U V
><<V W
spec<<X \
,<<\ ]
CancellationToken<<^ o
cancellationToken	<<p �
=
<<� �
default
<<� �
)
<<� �
{== 	
return@@ 
await@@ 
ApplySpecification@@ +
(@@+ ,
spec@@, 0
)@@0 1
.@@1 2
ToListAsync@@2 =
(@@= >
cancellationToken@@> O
)@@O P
;@@P Q
}AA 	
publicDD 
virtualDD 
asyncDD 
TaskDD !
<DD! "
intDD" %
>DD% &

CountAsyncDD' 1
(DD1 2
ISpecificationDD2 @
<DD@ A
TDDA B
>DDB C
specDDD H
,DDH I
CancellationTokenDDJ [
cancellationTokenDD\ m
=DDn o
defaultDDp w
)DDw x
{EE 	
returnHH 
awaitHH 
ApplySpecificationHH +
(HH+ ,
specHH, 0
,HH0 1 
evaluateCriteriaOnlyHH2 F
:HHF G
trueHHH L
)HHL M
.HHM N

CountAsyncHHN X
(HHX Y
cancellationTokenHHY j
)HHj k
;HHk l
}II 	
publicLL 
virtualLL 
asyncLL 
TaskLL !
<LL! "
TLL" #
>LL# $
AddAsyncLL% -
(LL- .
TLL. /
entityLL0 6
,LL6 7
CancellationTokenLL8 I
cancellationTokenLLJ [
=LL\ ]
defaultLL^ e
)LLe f
{MM 	
awaitNN 
_dbSetNN 
.NN 
AddAsyncNN !
(NN! "
entityNN" (
,NN( )
cancellationTokenNN* ;
)NN; <
;NN< =
returnTT 
entityTT 
;TT 
}UU 	
publicXX 
virtualXX 
asyncXX 
TaskXX !
<XX! "
IEnumerableXX" -
<XX- .
TXX. /
>XX/ 0
>XX0 1
GetAllAsyncXX2 =
(XX= >
CancellationTokenXX> O
cancellationTokenXXP a
=XXb c
defaultXXd k
)XXk l
{YY 	
return[[ 
await[[ 
_dbSet[[ 
.[[  
Where[[  %
([[% &
e[[& '
=>[[( *
![[+ ,
e[[, -
.[[- .
	IsDeleted[[. 7
)[[7 8
.[[8 9
ToListAsync[[9 D
([[D E
cancellationToken[[E V
)[[V W
;[[W X
}\\ 	
public__ 
virtual__ 
async__ 
Task__ !
AddRangeAsync__" /
(__/ 0
IEnumerable__0 ;
<__; <
T__< =
>__= >
entities__? G
,__G H
CancellationToken__I Z
cancellationToken__[ l
=__m n
default__o v
)__v w
{`` 	
awaitaa 
_dbSetaa 
.aa 
AddRangeAsyncaa &
(aa& '
entitiesaa' /
,aa/ 0
cancellationTokenaa1 B
)aaB C
;aaC D
}bb 	
publicee 
virtualee 
voidee 
Updateee "
(ee" #
Tee# $
entityee% +
)ee+ ,
{ff 	
ifhh 
(hh 
entityhh 
==hh 
nullhh 
)hh  
{ii 
throwjj 
newjj !
ArgumentNullExceptionjj 0
(jj0 1
nameofjj1 7
(jj7 8
entityjj8 >
)jj> ?
,jj? @
$strjjA c
)jjc d
;jjd e
}kk 
_contextll 
.ll 
Entryll 
(ll 
entityll !
)ll! "
.ll" #
Statell# (
=ll) *
EntityStatell+ 6
.ll6 7
Modifiedll7 ?
;ll? @
}mm 	
publicpp 
virtualpp 
Taskpp 
UpdateAsyncpp '
(pp' (
Tpp( )
entitypp* 0
,pp0 1
CancellationTokenpp2 C
cancellationTokenppD U
=ppV W
defaultppX _
)pp_ `
{qq 	
Updatess 
(ss 
entityss 
)ss 
;ss 
returnvv 
Taskvv 
.vv 
CompletedTaskvv %
;vv% &
}ww 	
publiczz 
virtualzz 
voidzz 
Deletezz "
(zz" #
Tzz# $
entityzz% +
)zz+ ,
{{{ 	
if}} 
(}} 
entity}} 
==}} 
null}} 
)}}  
{~~ 
throw 
new !
ArgumentNullException 0
(0 1
nameof1 7
(7 8
entity8 >
)> ?
,? @
$strA c
)c d
;d e
}
�� 
_dbSet
�� 
.
�� 
Remove
�� 
(
�� 
entity
��  
)
��  !
;
��! "
}
�� 	
public
�� 
virtual
�� 
Task
�� 
DeleteAsync
�� '
(
��' (
T
��( )
entity
��* 0
,
��0 1
CancellationToken
��2 C
cancellationToken
��D U
=
��V W
default
��X _
)
��_ `
{
�� 	
Delete
�� 
(
�� 
entity
�� 
)
�� 
;
�� 
return
�� 
Task
�� 
.
�� 
CompletedTask
�� %
;
��% &
}
�� 	
public
�� 
virtual
�� 
void
�� 
DeleteRange
�� '
(
��' (
IEnumerable
��( 3
<
��3 4
T
��4 5
>
��5 6
entities
��7 ?
)
��? @
{
�� 	
_dbSet
�� 
.
�� 
RemoveRange
�� 
(
�� 
entities
�� '
)
��' (
;
��( )
}
�� 	
public
�� 
virtual
�� 
async
�� 
Task
�� !
<
��! "
int
��" %
>
��% &
SaveChangesAsync
��' 7
(
��7 8
CancellationToken
��8 I
cancellationToken
��J [
=
��\ ]
default
��^ e
)
��e f
{
�� 	
return
�� 
await
�� 
_context
�� !
.
��! "
SaveChangesAsync
��" 2
(
��2 3
cancellationToken
��3 D
)
��D E
;
��E F
}
�� 	
	protected
�� 

IQueryable
�� 
<
�� 
T
�� 
>
��  
ApplySpecification
��  2
(
��2 3
ISpecification
��3 A
<
��A B
T
��B C
>
��C D
spec
��E I
,
��I J
bool
��K O"
evaluateCriteriaOnly
��P d
=
��e f
false
��g l
)
��l m
{
�� 	
return
�� $
SpecificationEvaluator
�� )
<
��) *
T
��* +
>
��+ ,
.
��, -
GetQuery
��- 5
(
��5 6
_dbSet
��6 <
.
��< =
AsQueryable
��= H
(
��H I
)
��I J
,
��J K
spec
��L P
)
��P Q
;
��Q R
}
�� 	
public
�� 
virtual
�� 
async
�� 
Task
�� !
<
��! "
bool
��" &
>
��& '
ExistsAsync
��( 3
(
��3 4

Expression
��4 >
<
��> ?
Func
��? C
<
��C D
T
��D E
,
��E F
bool
��G K
>
��K L
>
��L M
	predicate
��N W
,
��W X
CancellationToken
��Y j
cancellationToken
��k |
=
��} ~
default�� �
)��� �
{
�� 	
return
�� 
await
�� 
_dbSet
��  
.
��  !
Where
��! &
(
��& '
e
��' (
=>
��) +
!
��, -
e
��- .
.
��. /
	IsDeleted
��/ 8
)
��8 9
.
��9 :
AnyAsync
��: B
(
��B C
	predicate
��C L
,
��L M
cancellationToken
��N _
)
��_ `
;
��` a
}
�� 	
}
�� 
}�� �
iC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Repositories\CategoryRepository.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Repositories$ 0
{ 
public 

class 
CategoryRepository #
:$ %
GenericRepository& 7
<7 8
Category8 @
>@ A
,A B
ICategoryRepositoryC V
{ 
public		 
CategoryRepository		 !
(		! " 
ApplicationDbContext		" 6
context		7 >
)		> ?
:		@ A
base		B F
(		F G
context		G N
)		N O
{

 	
} 	
} 
} �
dC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\DesignTimeDbContextFactory.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
{ 
public 

class &
DesignTimeDbContextFactory +
:, -'
IDesignTimeDbContextFactory. I
<I J 
ApplicationDbContextJ ^
>^ _
{		 
public

  
ApplicationDbContext

 #
CreateDbContext

$ 3
(

3 4
string

4 :
[

: ;
]

; <
args

= A
)

A B
{ 	
string 
apiProjectPath !
=" #
Path$ (
.( )
Combine) 0
(0 1
	Directory1 :
.: ;
GetCurrentDirectory; N
(N O
)O P
,P Q
$strR `
)` a
;a b
var 
configuration 
= 
new  # 
ConfigurationBuilder$ 8
(8 9
)9 :
. 
SetBasePath 
( 
apiProjectPath +
)+ ,
. 
AddJsonFile 
( 
$str /
)/ 0
. 
AddJsonFile 
( 
$str ;
,; <
optional= E
:E F
trueG K
)K L
. 
Build 
( 
) 
; 
var 
builder 
= 
new #
DbContextOptionsBuilder 5
<5 6 
ApplicationDbContext6 J
>J K
(K L
)L M
;M N
var 
connectionString  
=! "
configuration# 0
.0 1
GetConnectionString1 D
(D E
$strE X
)X Y
;Y Z
builder 
. 
	UseNpgsql 
( 
connectionString .
). /
;/ 0
return 
new  
ApplicationDbContext +
(+ ,
builder, 3
.3 4
Options4 ;
); <
;< =
} 	
} 
} �
lC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Config\UserPermissionConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Config$ *
{ 
public 

class '
UserPermissionConfiguration ,
:- .$
IEntityTypeConfiguration/ G
<G H
UserPermissionH V
>V W
{		 
public

 
void

 
	Configure

 
(

 
EntityTypeBuilder

 /
<

/ 0
UserPermission

0 >
>

> ?
builder

@ G
)

G H
{ 	
builder 
. 
HasKey 
( 
x 
=> 
x  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
UserId$ *
)* +
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
PermissionId$ 0
)0 1
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
	IsGranted$ -
)- .
. 

IsRequired 
( 
) 
. 
HasDefaultValue  
(  !
true! %
)% &
;& '
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
	CreatedAt$ -
)- .
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
	UpdatedAt$ -
)- .
;. /
builder 
. 
HasOne 
( 
x 
=> 
x  !
.! "
User" &
)& '
. 
WithMany 
( 
x 
=> 
x  
.  !
UserPermissions! 0
)0 1
. 
HasForeignKey 
( 
x  
=>! #
x$ %
.% &
UserId& ,
), -
.   
OnDelete   
(   
DeleteBehavior   (
.  ( )
Cascade  ) 0
)  0 1
;  1 2
builder"" 
."" 
HasOne"" 
("" 
x"" 
=>"" 
x""  !
.""! "

Permission""" ,
)"", -
.## 
WithMany## 
(## 
x## 
=>## 
x##  
.##  !
UserPermissions##! 0
)##0 1
.$$ 
HasForeignKey$$ 
($$ 
x$$  
=>$$! #
x$$$ %
.$$% &
PermissionId$$& 2
)$$2 3
.%% 
OnDelete%% 
(%% 
DeleteBehavior%% (
.%%( )
Cascade%%) 0
)%%0 1
;%%1 2
builder'' 
.'' 
HasIndex'' 
('' 
x'' 
=>'' !
new''" %
{''& '
x''( )
.'') *
UserId''* 0
,''0 1
x''2 3
.''3 4
PermissionId''4 @
}''A B
)''B C
.(( 
IsUnique(( 
((( 
)(( 
;(( 
})) 	
}** 
}++ � 
bC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Config\UserConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Config$ *
{ 
public 

class 
UserConfiguration "
:# $$
IEntityTypeConfiguration% =
<= >
User> B
>B C
{ 
public		 
void		 
	Configure		 
(		 
EntityTypeBuilder		 /
<		/ 0
User		0 4
>		4 5
builder		6 =
)		= >
{

 	
builder 
. 
HasKey 
( 
x 
=> 
x  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
Adi$ '
)' (
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
Soyadi$ *
)* +
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
PasswordHash$ 0
)0 1
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
IsAdmin$ +
)+ ,
. 
HasDefaultValue  
(  !
false! &
)& '
;' (
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
Sicil$ )
)) *
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
	CreatedAt$ -
)- .
.   

IsRequired   
(   
)   
;   
builder"" 
."" 
Property"" 
("" 
x"" 
=>"" !
x""" #
.""# $
	CreatedBy""$ -
)""- .
.## 
HasMaxLength## 
(## 
$num##  
)##  !
;##! "
builder%% 
.%% 
Property%% 
(%% 
x%% 
=>%% !
x%%" #
.%%# $
	UpdatedBy%%$ -
)%%- .
.&& 
HasMaxLength&& 
(&& 
$num&&  
)&&  !
;&&! "
builder(( 
.(( 
HasOne(( 
((( 
x(( 
=>(( 
x((  !
.((! "
Role((" &
)((& '
.)) 
WithMany)) 
()) 
x)) 
=>)) 
x))  
.))  !
Users))! &
)))& '
.** 
HasForeignKey** 
(** 
x**  
=>**! #
x**$ %
.**% &
RoleId**& ,
)**, -
.++ 
OnDelete++ 
(++ 
DeleteBehavior++ (
.++( )
Restrict++) 1
)++1 2
;++2 3
builder-- 
.-- 
HasIndex-- 
(-- 
x-- 
=>-- !
x--" #
.--# $
Sicil--$ )
)--) *
... 
IsUnique.. 
(.. 
).. 
.// 
	HasFilter// 
(// 
$str// 2
)//2 3
;//3 4
}00 	
}11 
}22 �
lC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Config\RolePermissionConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Config$ *
{ 
public 

class '
RolePermissionConfiguration ,
:- .$
IEntityTypeConfiguration/ G
<G H
RolePermissionH V
>V W
{		 
public

 
void

 
	Configure

 
(

 
EntityTypeBuilder

 /
<

/ 0
RolePermission

0 >
>

> ?
builder

@ G
)

G H
{ 	
builder 
. 
HasKey 
( 
x 
=> 
x  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
RoleId$ *
)* +
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
PermissionId$ 0
)0 1
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
	CreatedAt$ -
)- .
. 

IsRequired 
( 
) 
; 
builder 
. 
HasOne 
( 
x 
=> 
x  !
.! "
Role" &
)& '
. 
WithMany 
( 
x 
=> 
x  
.  !
RolePermissions! 0
)0 1
. 
HasForeignKey 
( 
x  
=>! #
x$ %
.% &
RoleId& ,
), -
. 
OnDelete 
( 
DeleteBehavior (
.( )
Cascade) 0
)0 1
;1 2
builder 
. 
HasOne 
( 
x 
=> 
x  !
.! "

Permission" ,
), -
. 
WithMany 
( 
x 
=> 
x  
.  !
RolePermissions! 0
)0 1
. 
HasForeignKey 
( 
x  
=>! #
x$ %
.% &
PermissionId& 2
)2 3
. 
OnDelete 
( 
DeleteBehavior (
.( )
Cascade) 0
)0 1
;1 2
builder!! 
.!! 
HasIndex!! 
(!! 
x!! 
=>!! !
new!!" %
{!!& '
x!!( )
.!!) *
RoleId!!* 0
,!!0 1
x!!2 3
.!!3 4
PermissionId!!4 @
}!!A B
)!!B C
."" 
IsUnique"" 
("" 
)"" 
;"" 
}## 	
}$$ 
}%% �
bC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Config\RoleConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Config$ *
{ 
public 

class 
RoleConfiguration "
:# $$
IEntityTypeConfiguration% =
<= >
Role> B
>B C
{ 
public		 
void		 
	Configure		 
(		 
EntityTypeBuilder		 /
<		/ 0
Role		0 4
>		4 5
builder		6 =
)		= >
{

 	
builder 
. 
HasKey 
( 
x 
=> 
x  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
Name$ (
)( )
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
Description$ /
)/ 0
. 
HasMaxLength 
( 
$num !
)! "
;" #
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
	CreatedAt$ -
)- .
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
	CreatedBy$ -
)- .
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
	UpdatedBy$ -
)- .
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
HasIndex 
( 
x 
=> !
x" #
.# $
Name$ (
)( )
. 
IsUnique 
( 
) 
. 
	HasFilter 
( 
$str 2
)2 3
;3 4
}   	
}!! 
}"" �
hC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Config\PermissionConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Config$ *
{ 
public 

class #
PermissionConfiguration (
:) *$
IEntityTypeConfiguration+ C
<C D

PermissionD N
>N O
{ 
public		 
void		 
	Configure		 
(		 
EntityTypeBuilder		 /
<		/ 0

Permission		0 :
>		: ;
builder		< C
)		C D
{

 	
builder 
. 
HasKey 
( 
x 
=> 
x  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
Name$ (
)( )
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num !
)! "
;" #
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
Description$ /
)/ 0
. 
HasMaxLength 
( 
$num !
)! "
;" #
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
ResourceType$ 0
)0 1
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
Group$ )
)) *
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
	CreatedAt$ -
)- .
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
x 
=> !
x" #
.# $
	UpdatedAt$ -
)- .
;. /
builder 
. 
HasIndex 
( 
x 
=> !
x" #
.# $
Name$ (
)( )
.   
IsUnique   
(   
)   
.!! 
	HasFilter!! 
(!! 
$str!! 2
)!!2 3
;!!3 4
}"" 	
}## 
}$$ �
fC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Config\CategoryConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Config$ *
{ 
public 

class !
CategoryConfiguration &
:' ($
IEntityTypeConfiguration) A
<A B
CategoryB J
>J K
{ 
public		 
void		 
	Configure		 
(		 
EntityTypeBuilder		 /
<		/ 0
Category		0 8
>		8 9
builder		: A
)		A B
{

 	
builder 
. 
ToTable 
( 
$str (
)( )
;) *
builder 
. 
HasKey 
( 
c 
=> 
c  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
c 
=>  "
c# $
.$ %
Name% )
)) *
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num !
)! "
;" #
builder 
. 
HasIndex 
( 
c 
=> !
c" #
.# $
Name$ (
)( )
.) *
IsUnique* 2
(2 3
)3 4
;4 5
builder 
. 
Property 
( 
c 
=>  "
c# $
.$ %
Description% 0
)0 1
. 
HasMaxLength 
( 
$num !
)! "
;" #
} 	
} 
} �
jC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Configurations\RoleConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Configurations$ 2
{ 
public 

class 
RoleConfiguration "
:# $$
IEntityTypeConfiguration% =
<= >
Role> B
>B C
{ 
public		 
void		 
	Configure		 
(		 
EntityTypeBuilder		 /
<		/ 0
Role		0 4
>		4 5
builder		6 =
)		= >
{

 	
builder 
. 
ToTable 
( 
$str #
)# $
;$ %
builder 
. 
HasKey 
( 
r 
=> 
r  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
r 
=> !
r" #
.# $
Name$ (
)( )
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
r 
=> !
r" #
.# $
Description$ /
)/ 0
. 
HasMaxLength 
( 
$num !
)! "
;" #
builder 
. 
Property 
( 
r 
=> !
r" #
.# $
	CreatedAt$ -
)- .
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
r 
=> !
r" #
.# $
	IsDeleted$ -
)- .
. 
HasDefaultValue  
(  !
false! &
)& '
;' (
} 	
} 
} �
jC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Configurations\UserConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Configurations$ 2
{ 
public 

class 
UserConfiguration "
:# $$
IEntityTypeConfiguration% =
<= >
User> B
>B C
{ 
public		 
void		 
	Configure		 
(		 
EntityTypeBuilder		 /
<		/ 0
User		0 4
>		4 5
builder		6 =
)		= >
{

 	
builder 
. 
ToTable 
( 
$str #
)# $
;$ %
builder 
. 
HasKey 
( 
u 
=> 
u  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
u 
=> !
u" #
.# $
PasswordHash$ 0
)0 1
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
u 
=> !
u" #
.# $
Sicil$ )
)) *
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
HasIndex 
( 
u 
=> !
u" #
.# $
Sicil$ )
)) *
.* +
IsUnique+ 3
(3 4
)4 5
;5 6
builder 
. 
Property 
( 
u 
=> !
u" #
.# $
IsAdmin$ +
)+ ,
. 
HasDefaultValue  
(  !
false! &
)& '
;' (
builder 
. 
Property 
( 
u 
=> !
u" #
.# $
	CreatedAt$ -
)- .
. 

IsRequired 
( 
) 
; 
builder 
. 
Property 
( 
u 
=> !
u" #
.# $
	IsDeleted$ -
)- .
.   
HasDefaultValue    
(    !
false  ! &
)  & '
;  ' (
builder## 
.## 
HasOne## 
(## 
u## 
=>## 
u##  !
.##! "
Role##" &
)##& '
.$$ 
WithMany$$ 
($$ 
r$$ 
=>$$ 
r$$  
.$$  !
Users$$! &
)$$& '
.%% 
HasForeignKey%% 
(%% 
u%%  
=>%%! #
u%%$ %
.%%% &
RoleId%%& ,
)%%, -
.&& 
OnDelete&& 
(&& 
DeleteBehavior&& (
.&&( )
SetNull&&) 0
)&&0 1
;&&1 2
}'' 	
}(( 
})) �
mC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Configurations\ProductConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Configurations$ 2
{ 
public 

class  
ProductConfiguration %
:& '$
IEntityTypeConfiguration( @
<@ A
ProductA H
>H I
{		 
public

 
void

 
	Configure

 
(

 
EntityTypeBuilder

 /
<

/ 0
Product

0 7
>

7 8
builder

9 @
)

@ A
{ 	
builder 
. 
HasKey 
( 
p 
=> 
p  !
.! "
Id" $
)$ %
;% &
builder 
. 
ComplexProperty #
(# $
p$ %
=>& (
p) *
.* +
Name+ /
,/ 0
nameBuilder1 <
=>= ?
{ 
nameBuilder 
. 
Property $
($ %
pn% '
=>( *
pn+ -
.- .
Value. 3
)3 4
. 

IsRequired 
(  
)  !
. 
HasMaxLength !
(! "
$num" %
)% &
;& '
} 
) 
; 
builder 
. 
ComplexProperty #
(# $
p$ %
=>& (
p) *
.* +
Description+ 6
,6 7
descBuilder8 C
=>D F
{ 
descBuilder 
. 
Property $
($ %
pd% '
=>( *
pd+ -
.- .
Value. 3
)3 4
. 
HasColumnName "
(" #
$str# 0
)0 1
. 

IsRequired 
(  
false  %
)% &
. 
HasMaxLength !
(! "
$num" %
)% &
;& '
} 
) 
; 
builder 
. 
ComplexProperty #
(# $
p$ %
=>& (
p) *
.* +

StockLevel+ 5
,5 6
stockBuilder7 C
=>D F
{   
stockBuilder"" 
."" 
Property"" %
(""% &
sl""& (
=>"") +
sl"", .
."". /
Value""/ 4
)""4 5
.## 
HasColumnName## "
(##" #
$str### 2
)##2 3
.$$ 

IsRequired$$ 
($$  
)$$  !
;$$! "
}%% 
)%% 
;%% 
builder(( 
.(( 
HasOne(( 
((( 
p(( 
=>(( 
p((  !
.((! "
Category((" *
)((* +
.)) 
WithMany)) 
()) 
))) 
.** 
HasForeignKey** 
(** 
p**  
=>**! #
p**$ %
.**% &

CategoryId**& 0
)**0 1
.++ 

IsRequired++ 
(++ 
)++ 
;++ 
builder.. 
... 
Property.. 
(.. 
p.. 
=>.. !
p.." #
...# $
	CreatedAt..$ -
)..- .
.... /

IsRequired../ 9
(..9 :
)..: ;
;..; <
builder// 
.// 
Property// 
(// 
p// 
=>// !
p//" #
.//# $
	UpdatedAt//$ -
)//- .
;//. /
}00 	
}11 
}22 �
pC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Configurations\PermissionConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Configurations$ 2
{ 
public 

class #
PermissionConfiguration (
:) *$
IEntityTypeConfiguration+ C
<C D

PermissionD N
>N O
{		 
public

 
void

 
	Configure

 
(

 
EntityTypeBuilder

 /
<

/ 0

Permission

0 :
>

: ;
builder

< C
)

C D
{ 	
builder 
. 
ToTable 
( 
$str )
)) *
;* +
builder 
. 
HasKey 
( 
p 
=> 
p  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
p 
=> !
p" #
.# $
Name$ (
)( )
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
p 
=> !
p" #
.# $
Description$ /
)/ 0
. 
HasMaxLength 
( 
$num !
)! "
;" #
builder 
. 
Property 
( 
p 
=> !
p" #
.# $
Group$ )
)) *
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
p 
=> !
p" #
.# $
ResourceType$ 0
)0 1
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
p 
=> !
p" #
.# $
ResourceName$ 0
)0 1
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder   
.   
Property   
(   
p   
=>   !
p  " #
.  # $
Action  $ *
)  * +
.!! 
HasMaxLength!! 
(!! 
$num!!  
)!!  !
;!!! "
builder## 
.## 
Property## 
(## 
p## 
=>## !
p##" #
.### $
	CreatedAt##$ -
)##- .
.$$ 

IsRequired$$ 
($$ 
)$$ 
;$$ 
builder&& 
.&& 
Property&& 
(&& 
p&& 
=>&& !
p&&" #
.&&# $
	IsDeleted&&$ -
)&&- .
.'' 
HasDefaultValue''  
(''  !
false''! &
)''& '
;''' (
builder** 
.** 
HasIndex** 
(** 
p** 
=>** !
p**" #
.**# $
Name**$ (
)**( )
.++ 
IsUnique++ 
(++ 
)++ 
;++ 
},, 	
}-- 
}.. � 
qC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Configurations\ActivityLogConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Configurations$ 2
{ 
public 

class $
ActivityLogConfiguration )
:* +$
IEntityTypeConfiguration, D
<D E
ActivityLogE P
>P Q
{ 
public		 
void		 
	Configure		 
(		 
EntityTypeBuilder		 /
<		/ 0
ActivityLog		0 ;
>		; <
builder		= D
)		D E
{

 	
builder 
. 
ToTable 
( 
$str *
)* +
;+ ,
builder 
. 
HasKey 
( 
a 
=> 
a  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
a 
=> !
a" #
.# $
UserId$ *
)* +
;+ ,
builder 
. 
Property 
( 
a 
=> !
a" #
.# $
Username$ ,
), -
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
a 
=> !
a" #
.# $
ActivityType$ 0
)0 1
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
a 
=> !
a" #
.# $
Description$ /
)/ 0
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num !
)! "
;" #
builder 
. 
Property 
( 
a 
=> !
a" #
.# $
	Timestamp$ -
)- .
. 

IsRequired 
( 
) 
; 
builder   
.   
Property   
(   
a   
=>   !
a  " #
.  # $
AdditionalInfo  $ 2
)  2 3
;  3 4
builder"" 
."" 
Property"" 
("" 
a"" 
=>"" !
a""" #
.""# $
IsSynchronized""$ 2
)""2 3
.## 
HasDefaultValue##  
(##  !
false##! &
)##& '
;##' (
builder%% 
.%% 
Property%% 
(%% 
a%% 
=>%% !
a%%" #
.%%# $
	CreatedAt%%$ -
)%%- .
.&& 

IsRequired&& 
(&& 
)&& 
;&& 
builder(( 
.(( 
Property(( 
((( 
a(( 
=>(( !
a((" #
.((# $
	IsDeleted(($ -
)((- .
.)) 
HasDefaultValue))  
())  !
false))! &
)))& '
;))' (
builder++ 
.++ 
HasOne++ 
(++ 
a++ 
=>++ 
a++  !
.++! "
User++" &
)++& '
.,, 
WithMany,, 
(,, 
),, 
.-- 
HasForeignKey-- 
(-- 
a--  
=>--! #
a--$ %
.--% &
UserId--& ,
)--, -
... 
OnDelete.. 
(.. 
DeleteBehavior.. (
...( )
SetNull..) 0
)..0 1
.// 

IsRequired// 
(// 
false// !
)//! "
;//" #
}00 	
}11 
}22 �
nC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\Configurations\AuditLogConfiguration.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
.# $
Configurations$ 2
{ 
public 

class !
AuditLogConfiguration &
:' ($
IEntityTypeConfiguration) A
<A B
AuditLogB J
>J K
{ 
public		 
void		 
	Configure		 
(		 
EntityTypeBuilder		 /
<		/ 0
AuditLog		0 8
>		8 9
builder		: A
)		A B
{

 	
builder 
. 
ToTable 
( 
$str '
)' (
;( )
builder 
. 
HasKey 
( 
a 
=> 
a  !
.! "
Id" $
)$ %
;% &
builder 
. 
Property 
( 
a 
=> !
a" #
.# $
Action$ *
)* +
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
a 
=> !
a" #
.# $

EntityType$ .
). /
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num !
)! "
;" #
builder 
. 
Property 
( 
a 
=> !
a" #
.# $
EntityId$ ,
), -
. 

IsRequired 
( 
) 
. 
HasMaxLength 
( 
$num  
)  !
;! "
builder 
. 
Property 
( 
a 
=> !
a" #
.# $
UserId$ *
)* +
;+ ,
builder 
. 
Property 
( 
a 
=> !
a" #
.# $
Path$ (
)( )
. 
HasMaxLength 
( 
$num !
)! "
;" #
builder   
.   
Property   
(   
a   
=>   !
a  " #
.  # $
	CreatedAt  $ -
)  - .
.!! 

IsRequired!! 
(!! 
)!! 
;!! 
builder## 
.## 
Property## 
(## 
a## 
=>## !
a##" #
.### $
	IsDeleted##$ -
)##- .
.$$ 
HasDefaultValue$$  
($$  !
false$$! &
)$$& '
;$$' (
builder'' 
.'' 
HasOne'' 
('' 
a'' 
=>'' 
a''  !
.''! "
User''" &
)''& '
.(( 
WithMany(( 
((( 
)(( 
.)) 
HasForeignKey)) 
()) 
a))  
=>))! #
a))$ %
.))% &
UserId))& ,
))), -
.** 

IsRequired** 
(** 
false** !
)**! "
.++ 
OnDelete++ 
(++ 
DeleteBehavior++ (
.++( )
SetNull++) 0
)++0 1
;++1 2
},, 	
}-- 
}.. �]
^C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Data\ApplicationDbContext.cs
	namespace 	
Stock
 
. 
Infrastructure 
. 
Data #
{ 
public 

class  
ApplicationDbContext %
:& '
	DbContext( 1
{ 
public  
ApplicationDbContext #
(# $
DbContextOptions$ 4
<4 5 
ApplicationDbContext5 I
>I J
optionsK R
)R S
: 
base 
( 
options 
) 
{ 	
} 	
public 
DbSet 
< 
User 
> 
Users  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
null1 5
!5 6
;6 7
public 
DbSet 
< 
Role 
> 
Roles  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
null1 5
!5 6
;6 7
public 
DbSet 
< 
AuditLog 
> 
	AuditLogs (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
=7 8
null9 =
!= >
;> ?
public 
DbSet 
< 
ActivityLog  
>  !
ActivityLogs" .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
public 
DbSet 
< 

Permission 
>  
Permissions! ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
=; <
null= A
!A B
;B C
public 
DbSet 
< 
RolePermission #
># $
RolePermissions% 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
=C D
nullE I
!I J
;J K
public 
DbSet 
< 
UserPermission #
># $
UserPermissions% 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
=C D
nullE I
!I J
;J K
public 
DbSet 
< 
Product 
> 
Products &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
=5 6
null7 ;
!; <
;< =
public   
DbSet   
<   
Category   
>   

Categories   )
{  * +
get  , /
;  / 0
set  1 4
;  4 5
}  6 7
=  8 9
null  : >
!  > ?
;  ? @
	protected"" 
override"" 
void"" 
OnModelCreating""  /
(""/ 0
ModelBuilder""0 <
modelBuilder""= I
)""I J
{## 	
base$$ 
.$$ 
OnModelCreating$$  
($$  !
modelBuilder$$! -
)$$- .
;$$. /
modelBuilder'' 
.'' +
ApplyConfigurationsFromAssembly'' 8
(''8 9
typeof''9 ?
(''? @ 
ApplicationDbContext''@ T
)''T U
.''U V
Assembly''V ^
)''^ _
;''_ `
SeedData** 
(** 
modelBuilder** !
)**! "
;**" #
}++ 	
private-- 
void-- 
SeedData-- 
(-- 
ModelBuilder-- *
modelBuilder--+ 7
)--7 8
{.. 	
var00 
seedDate00 
=00 
new00 
DateTime00 '
(00' (
$num00( ,
,00, -
$num00. /
,00/ 0
$num001 2
,002 3
$num004 5
,005 6
$num007 8
,008 9
$num00: ;
,00; <
DateTimeKind00= I
.00I J
Utc00J M
)00M N
;00N O
modelBuilder33 
.33 
Entity33 
<33  
Role33  $
>33$ %
(33% &
)33& '
.33' (
HasData33( /
(33/ 0
new44 
Role44 
{55 
Id66 
=66 
$num66 
,66 
Name77 
=77 
$str77 "
,77" #
Description88 
=88  !
$str88" G
,88G H
	CreatedAt99 
=99 
seedDate99  (
,99( )
	IsDeleted:: 
=:: 
false::  %
};; 
)<< 
;<< 
modelBuilder?? 
.?? 
Entity?? 
<??  
User??  $
>??$ %
(??% &
)??& '
.??' (
HasData??( /
(??/ 0
new@@ 
User@@ 
{AA 
IdBB 
=BB 
$numBB 
,BB 
AdiCC 
=CC 
$strCC !
,CC! "
SoyadiDD 
=DD 
$strDD *
,DD* +
SicilEE 
=EE 
$strEE #
,EE# $
PasswordHashFF  
=FF! "
$strFF# a
,FFa b
IsAdminGG 
=GG 
trueGG "
,GG" #
RoleIdHH 
=HH 
$numHH 
,HH 
	CreatedAtII 
=II 
seedDateII  (
,II( )
	IsDeletedJJ 
=JJ 
falseJJ  %
}KK 
)LL 
;LL 
varOO 
allPermissionFieldsOO #
=OO$ %
typeofOO& ,
(OO, -
PermissionNamesOO- <
)OO< =
.OO= >
GetNestedTypesOO> L
(OOL M
)OOM N
.PP 

SelectManyPP 
(PP 
typePP  
=>PP! #
typePP$ (
.PP( )
	GetFieldsPP) 2
(PP2 3
SystemPP3 9
.PP9 :

ReflectionPP: D
.PPD E
BindingFlagsPPE Q
.PPQ R
PublicPPR X
|PPY Z
SystemPP[ a
.PPa b

ReflectionPPb l
.PPl m
BindingFlagsPPm y
.PPy z
Static	PPz �
)
PP� �
)
PP� �
.QQ 
WhereQQ 
(QQ 
fQQ 
=>QQ 
fQQ 
.QQ 
	IsLiteralQQ '
&&QQ( *
!QQ+ ,
fQQ, -
.QQ- .

IsInitOnlyQQ. 8
&&QQ9 ;
fQQ< =
.QQ= >
	FieldTypeQQ> G
==QQH J
typeofQQK Q
(QQQ R
stringQQR X
)QQX Y
)QQY Z
.RR 
ToListRR 
(RR 
)RR 
;RR 
intTT 
permissionIdCounterTT #
=TT$ %
$numTT& '
;TT' (
varUU 
permissionsToSeedUU !
=UU" #
newUU$ '
ListUU( ,
<UU, -

PermissionUU- 7
>UU7 8
(UU8 9
)UU9 :
;UU: ;
varVV !
permissionNameToIdMapVV %
=VV& '
newVV( +

DictionaryVV, 6
<VV6 7
stringVV7 =
,VV= >
intVV? B
>VVB C
(VVC D
)VVD E
;VVE F
foreachXX 
(XX 
varXX 
fieldXX 
inXX !
allPermissionFieldsXX" 5
)XX5 6
{YY 
stringZZ 
permissionNameZZ %
=ZZ& '
(ZZ( )
stringZZ) /
)ZZ/ 0
fieldZZ0 5
.ZZ5 6
GetValueZZ6 >
(ZZ> ?
nullZZ? C
)ZZC D
;ZZD E
string[[ 
	groupName[[  
=[[! "
field[[# (
.[[( )
DeclaringType[[) 6
.[[6 7
Name[[7 ;
;[[; <
permissionsToSeed\\ !
.\\! "
Add\\" %
(\\% &
new\\& )

Permission\\* 4
{]] 
Id^^ 
=^^ 
permissionIdCounter^^ ,
,^^, -
Name__ 
=__ 
permissionName__ )
,__) *
Group`` 
=`` 
	groupName`` %
,``% &
Descriptionaa 
=aa  !
$"aa" $
{aa$ %
	groupNameaa% .
}aa. /
$straa/ 2
{aa2 3
permissionNameaa3 A
}aaA B
"aaB C
,aaC D
	CreatedAtbb 
=bb 
seedDatebb  (
,bb( )
	IsDeletedcc 
=cc 
falsecc  %
}dd 
)dd 
;dd !
permissionNameToIdMapee %
[ee% &
permissionNameee& 4
]ee4 5
=ee6 7
permissionIdCounteree8 K
;eeK L
permissionIdCounterff #
++ff# %
;ff% &
}gg 
modelBuilderhh 
.hh 
Entityhh 
<hh  

Permissionhh  *
>hh* +
(hh+ ,
)hh, -
.hh- .
HasDatahh. 5
(hh5 6
permissionsToSeedhh6 G
)hhG H
;hhH I
varkk  
adminRolePermissionskk $
=kk% &
permissionsToSeedkk' 8
.kk8 9
Selectkk9 ?
(kk? @
pkk@ A
=>kkB D
newkkE H
RolePermissionkkI W
{ll 
Idmm 
=mm 
pmm 
.mm 
Idmm 
,mm 
RoleIdnn 
=nn 
$numnn 
,nn 
PermissionIdoo 
=oo 
poo  
.oo  !
Idoo! #
,oo# $
	CreatedAtpp 
=pp 
seedDatepp $
,pp$ %
	IsDeletedqq 
=qq 
falseqq !
}rr 
)rr 
.rr 
ToListrr 
(rr 
)rr 
;rr 
modelBuildertt 
.tt 
Entitytt 
<tt  
RolePermissiontt  .
>tt. /
(tt/ 0
)tt0 1
.tt1 2
HasDatatt2 9
(tt9 : 
adminRolePermissionstt: N
)ttN O
;ttO P
}uu 	
publicww 
overrideww 
Taskww 
<ww 
intww  
>ww  !
SaveChangesAsyncww" 2
(ww2 3
CancellationTokenww3 D
cancellationTokenwwE V
=wwW X
defaultwwY `
)ww` a
{xx 	
varyy 
entriesyy 
=yy 
ChangeTrackeryy '
.yy' (
Entriesyy( /
<yy/ 0

BaseEntityyy0 :
>yy: ;
(yy; <
)yy< =
;yy= >
foreach{{ 
({{ 
var{{ 
entry{{ 
in{{ !
entries{{" )
){{) *
{|| 
switch}} 
(}} 
entry}} 
.}} 
State}} #
)}}# $
{~~ 
case 
EntityState $
.$ %
Added% *
:* +
entry
�� 
.
�� 
Entity
�� $
.
��$ %
	CreatedAt
��% .
=
��/ 0
DateTime
��1 9
.
��9 :
UtcNow
��: @
;
��@ A
break
�� 
;
�� 
case
�� 
EntityState
�� $
.
��$ %
Modified
��% -
:
��- .
entry
�� 
.
�� 
Entity
�� $
.
��$ %
	UpdatedAt
��% .
=
��/ 0
DateTime
��1 9
.
��9 :
UtcNow
��: @
;
��@ A
break
�� 
;
�� 
}
�� 
}
�� 
return
�� 
base
�� 
.
�� 
SaveChangesAsync
�� (
(
��( )
cancellationToken
��) :
)
��: ;
;
��; <
}
�� 	
}
�� 
}�� �
KC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Class1.cs
	namespace 	
Stock
 
. 
Infrastructure 
; 
public 
class 
Class1 
{ 
} 