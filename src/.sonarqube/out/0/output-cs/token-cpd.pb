“
TC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\ValueObjects\StockLevel.cs
	namespace 	
Stock
 
. 
Domain 
. 
ValueObjects #
{ 
public 

class 

StockLevel 
: 
ValueObject )
{ 
public		 
int		 
Value		 
{		 
get		 
;		 
}		  !
private 

StockLevel 
( 
int 
value $
)$ %
{ 	
if 
( 
value 
< 
$num 
) 
{ 
throw 
new 
DomainException )
() *
$str* K
)K L
;L M
} 
Value 
= 
value 
; 
} 	
public 
static 

StockLevel  
From! %
(% &
int& )
value* /
)/ 0
{ 	
return 
new 

StockLevel !
(! "
value" '
)' (
;( )
} 	
public 

StockLevel 
Increase "
(" #
int# &
amount' -
)- .
{ 	
if 
( 
amount 
< 
$num 
) 
throw 
new 
DomainException )
() *
$str* T
)T U
;U V
return 
new 

StockLevel !
(! "
Value" '
+( )
amount* 0
)0 1
;1 2
} 	
public!! 

StockLevel!! 
Decrease!! "
(!!" #
int!!# &
amount!!' -
)!!- .
{"" 	
if## 
(## 
amount## 
<## 
$num## 
)## 
throw$$ 
new$$ 
DomainException$$ )
($$) *
$str$$* T
)$$T U
;$$U V
if%% 
(%% 
Value%% 
-%% 
amount%% 
<%%  
$num%%! "
)%%" #
throw&& 
new&& 
DomainException&& )
(&&) *
$str&&* S
)&&S T
;&&T U
return'' 
new'' 

StockLevel'' !
(''! "
Value''" '
-''( )
amount''* 0
)''0 1
;''1 2
}(( 	
	protected** 
override** 
IEnumerable** &
<**& '
object**' -
>**- .!
GetEqualityComponents**/ D
(**D E
)**E F
{++ 	
yield,, 
return,, 
Value,, 
;,, 
}-- 	
public// 
static// 
implicit// 
operator// '
int//( +
(//+ ,

StockLevel//, 6
level//7 <
)//< =
=>//> @
level//A F
.//F G
Value//G L
;//L M
}00 
}11 —
UC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\ValueObjects\ProductName.cs
	namespace 	
Stock
 
. 
Domain 
. 
ValueObjects #
{ 
public 

class 
ProductName 
: 
ValueObject *
{ 
public		 
string		 
Value		 
{		 
get		 !
;		! "
}		# $
private 
ProductName 
( 
string "
value# (
)( )
{ 	
if 
( 
string 
. 
IsNullOrWhiteSpace )
() *
value* /
)/ 0
)0 1
{ 
throw 
new 
DomainException )
() *
$str* W
)W X
;X Y
} 
Value 
= 
value 
; 
} 	
public 
static 
ProductName !
From" &
(& '
string' -
value. 3
)3 4
{ 	
return 
new 
ProductName "
(" #
value# (
)( )
;) *
} 	
	protected 
override 
IEnumerable &
<& '
object' -
>- .!
GetEqualityComponents/ D
(D E
)E F
{ 	
yield   
return   
Value   
;   
}!! 	
public$$ 
static$$ 
implicit$$ 
operator$$ '
string$$( .
($$. /
ProductName$$/ :
name$$; ?
)$$? @
=>$$A C
name$$D H
.$$H I
Value$$I N
;$$N O
}%% 
}&& ï
\C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\ValueObjects\ProductDescription.cs
	namespace 	
Stock
 
. 
Domain 
. 
ValueObjects #
{ 
public 

class 
ProductDescription #
:$ %
ValueObject& 1
{ 
private		 
const		 
int		 
	MaxLength		 #
=		$ %
$num		& )
;		) *
public

 
string

 
Value

 
{

 
get

 !
;

! "
}

# $
private 
ProductDescription "
(" #
string# )
value* /
)/ 0
{ 	
if 
( 
value 
!= 
null 
&&  
value! &
.& '
Length' -
>. /
	MaxLength0 9
)9 :
{ 
throw 
new 
DomainException )
() *
$"* ,
$str, N
{N O
	MaxLengthO X
}X Y
$strY e
"e f
)f g
;g h
} 
Value 
= 
value 
; 
} 	
public 
static 
ProductDescription (
From) -
(- .
string. 4
value5 :
): ;
{ 	
return 
new 
ProductDescription )
() *
value* /
)/ 0
;0 1
} 	
	protected 
override 
IEnumerable &
<& '
object' -
>- .!
GetEqualityComponents/ D
(D E
)E F
{ 	
yield 
return 
Value 
; 
} 	
public   
static   
implicit   
operator   '
string  ( .
(  . /
ProductDescription  / A
description  B M
)  M N
=>  O Q
description  R ]
?  ] ^
.  ^ _
Value  _ d
;  d e
public"" 
override"" 
string"" 
ToString"" '
(""' (
)""( )
=>""* ,
Value""- 2
??""3 5
string""6 <
.""< =
Empty""= B
;""B C
}## 
}$$ ¶
yC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\Users\UserWithRoleAndPermissionsSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
.% &
Users& +
{ 
public 

class 3
'UserWithRoleAndPermissionsSpecification 8
:9 :
BaseSpecification; L
<L M
UserM Q
>Q R
{ 
public		 3
'UserWithRoleAndPermissionsSpecification		 6
(		6 7
int		7 :
userId		; A
)		A B
:		C D
base		E I
(		I J
u		J K
=>		L N
u		O P
.		P Q
Id		Q S
==		T V
userId		W ]
)		] ^
{

 	

AddInclude 
( 
u 
=> 
u 
. 
Role "
)" #
;# $

AddInclude 
( 
$" 
{ 
nameof  
(  !
User! %
.% &
Role& *
)* +
}+ ,
$str, -
{- .
nameof. 4
(4 5
Role5 9
.9 :
RolePermissions: I
)I J
}J K
"K L
)L M
;M N

AddInclude 
( 
$" 
{ 
nameof  
(  !
User! %
.% &
Role& *
)* +
}+ ,
$str, -
{- .
nameof. 4
(4 5
Role5 9
.9 :
RolePermissions: I
)I J
}J K
$strK L
{L M
nameofM S
(S T
RolePermissionT b
.b c

Permissionc m
)m n
}n o
"o p
)p q
;q r
} 	
} 
} ∫
gC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\UsersWithRolesSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public		 

sealed		 
class		 '
UsersWithRolesSpecification		 3
:		4 5
BaseSpecification		6 G
<		G H
User		H L
>		L M
{

 
public '
UsersWithRolesSpecification *
(* +
bool+ /
orderByAdiSoyadi0 @
=A B
falseC H
,H I
intJ M
?M N
skipO S
=T U
nullV Z
,Z [
int\ _
?_ `
takea e
=f g
nullh l
)l m
: 
base 
( 
) 
{ 	

AddInclude 
( 
u 
=> 
u 
. 
Role "
)" #
;# $
if 
( 
orderByAdiSoyadi  
)  !
{ 
ApplyOrderBy 
( 
u 
=> !
u" #
.# $
Adi$ '
)' (
;( )
ApplyThenBy 
( 
u 
=>  
u! "
." #
Soyadi# )
)) *
;* +
} 
if 
( 
skip 
. 
HasValue 
&&  
take! %
.% &
HasValue& .
). /
{ 
ApplyPaging 
( 
skip  
.  !
Value! &
,& '
take( ,
., -
Value- 2
)2 3
;3 4
} 
}   	
public&& '
UsersWithRolesSpecification&& *
(&&* +
int&&+ .
userId&&/ 5
)&&5 6
:'' 
base'' 
('' 
u'' 
=>'' 
u'' 
.'' 
Id'' 
=='' 
userId''  &
)''& '
{(( 	

AddInclude)) 
()) 
u)) 
=>)) 
u)) 
.)) 
Role)) "
)))" #
;))# $
}** 	
}++ 
},, †
ÉC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\UserPermissions\UserPermissionsWithDetailsSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
.% &
UserPermissions& 5
{ 
public 

class 3
'UserPermissionsWithDetailsSpecification 8
:9 :
BaseSpecification; L
<L M
UserPermissionM [
>[ \
{ 
public 3
'UserPermissionsWithDetailsSpecification 6
(6 7
int7 :
userId; A
)A B
:C D
baseE I
(I J
upJ L
=>M O
upP R
.R S
UserIdS Y
==Z \
userId] c
)c d
{		 	

AddInclude

 
(

 
up

 
=>

 
up

 
.

  

Permission

  *
)

* +
;

+ ,
} 	
} 
} å
wC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\UserPermissions\UserPermissionSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
.% &
UserPermissions& 5
{ 
public 

class '
UserPermissionSpecification ,
:- .
BaseSpecification/ @
<@ A
UserPermissionA O
>O P
{ 
public		 '
UserPermissionSpecification		 *
(		* +
int		+ .
userId		/ 5
,		5 6
int		7 :
permissionId		; G
)		G H
:

 
base

 
(

 
up

 
=>

 
up

 
.

 
UserId

 "
==

# %
userId

& ,
&&

- /
up

0 2
.

2 3
PermissionId

3 ?
==

@ B
permissionId

C O
)

O P
{ 	
} 	
} 
} ó
ÄC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\UserPermissions\UserPermissionsByUserIdSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
.% &
UserPermissions& 5
{ 
public 

class 0
$UserPermissionsByUserIdSpecification 5
:6 7
BaseSpecification8 I
<I J
UserPermissionJ X
>X Y
{ 
public		 0
$UserPermissionsByUserIdSpecification		 3
(		3 4
int		4 7
userId		8 >
)		> ?
:

 
base

 
(

 
up

 
=>

 
up

 
.

 
UserId

 "
==

# %
userId

& ,
)

, -
{ 	

AddInclude 
( 
up 
=> 
up 
.  

Permission  *
)* +
;+ ,
} 	
} 
} ‡	
}C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\UserPermissions\UserPermissionByNameSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
.% &
UserPermissions& 5
{ 
public 

class -
!UserPermissionByNameSpecification 2
:3 4
BaseSpecification5 F
<F G
UserPermissionG U
>U V
{ 
public		 -
!UserPermissionByNameSpecification		 0
(		0 1
int		1 4
userId		5 ;
,		; <
string		= C
permissionName		D R
)		R S
:

 
base

 
(

 
up

 
=>

 
up

 
.

 
UserId

 "
==

# %
userId

& ,
&&

- /
up

0 2
.

2 3

Permission

3 =
.

= >
Name

> B
==

C E
permissionName

F T
)

T U
{ 	

AddInclude 
( 
up 
=> 
up 
.  

Permission  *
)* +
;+ ,
} 	
} 
} ¶	
~C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\UserPermissions\UserCustomPermissionsSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
.% &
UserPermissions& 5
{ 
public 

class .
"UserCustomPermissionsSpecification 3
:4 5
BaseSpecification6 G
<G H
UserPermissionH V
>V W
{ 
public		 .
"UserCustomPermissionsSpecification		 1
(		1 2
int		2 5
userId		6 <
)		< =
:

 
base

 
(

 
up

 
=>

 
up

 
.

 
UserId

 "
==

# %
userId

& ,
)

, -
{ 	

AddInclude 
( 
up 
=> 
up 
.  

Permission  *
)* +
;+ ,

AddInclude 
( 
up 
=> 
up 
.  
User  $
)$ %
;% &
} 	
} 
} ˆ
dC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\UserBySicilSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public

 

sealed

 
class

 $
UserBySicilSpecification

 0
:

1 2
BaseSpecification

3 D
<

D E
User

E I
>

I J
{ 
public $
UserBySicilSpecification '
(' (
string( .
sicil/ 4
,4 5
bool6 :
includeRole; F
=G H
trueI M
)M N
: 
base 
( 
u 
=> 
u 
. 
Sicil 
==  "
sicil# (
)( )
{ 	
if 
( 
includeRole 
) 
{ 

AddInclude 
( 
u 
=> 
u  !
.! "
Role" &
)& '
;' (
} 
} 	
} 
} Î
oC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\UserBySicilExcludingIdSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public

 

sealed

 
class

 /
#UserBySicilExcludingIdSpecification

 ;
:

< =
BaseSpecification

> O
<

O P
User

P T
>

T U
{ 
public /
#UserBySicilExcludingIdSpecification 2
(2 3
string3 9
sicil: ?
,? @
intA D
userIdToExcludeE T
)T U
: 
base 
( 
u 
=> 
u 
. 
Sicil 
==  "
sicil# (
&&) +
u, -
.- .
Id. 0
!=1 3
userIdToExclude4 C
)C D
{ 	
} 	
} 
} †
eC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\UserByRoleIdSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public		 

class		 %
UserByRoleIdSpecification		 *
:		+ ,
BaseSpecification		- >
<		> ?
User		? C
>		C D
{

 
public %
UserByRoleIdSpecification (
(( )
int) ,
?, -
roleId. 4
)4 5
: 
base 
( 
u 
=> 
u 
. 
RoleId  
==! #
roleId$ *
)* +
{ 	
} 	
} 
} ï
aC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\UserByIdSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public

 

sealed

 
class

 !
UserByIdSpecification

 -
:

. /
BaseSpecification

0 A
<

A B
User

B F
>

F G
{ 
public !
UserByIdSpecification $
($ %
int% (
userId) /
)/ 0
: 
base 
( 
u 
=> 
u 
. 
Id 
== 
userId  &
)& '
{ 	
} 	
} 
} º
gC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\RolesWithUsersSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public 

sealed 
class '
RolesWithUsersSpecification 3
:4 5
BaseSpecification6 G
<G H
RoleH L
>L M
{		 
public '
RolesWithUsersSpecification *
(* +
)+ ,
: 
base 
( 
) 
{ 	

AddInclude 
( 
r 
=> 
r 
. 
Users #
)# $
;$ %
ApplyOrderBy 
( 
r 
=> 
r 
.  
Name  $
)$ %
;% &
} 	
} 
} ˚
ÄC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\RolePermissions\RolePermissionsByRoleIdSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
.% &
RolePermissions& 5
{ 
public 

class 0
$RolePermissionsByRoleIdSpecification 5
:6 7
BaseSpecification8 I
<I J
RolePermissionJ X
>X Y
{ 
public		 0
$RolePermissionsByRoleIdSpecification		 3
(		3 4
int		4 7
roleId		8 >
)		> ?
:

 
base

 
(

 
rp

 
=>

 
rp

 
.

 
RoleId

 "
==

# %
roleId

& ,
)

, -
{ 	
} 	
} 
} ‡	
}C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\RolePermissions\RolePermissionByNameSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
.% &
RolePermissions& 5
{ 
public 

class -
!RolePermissionByNameSpecification 2
:3 4
BaseSpecification5 F
<F G
RolePermissionG U
>U V
{ 
public		 -
!RolePermissionByNameSpecification		 0
(		0 1
int		1 4
roleId		5 ;
,		; <
string		= C
permissionName		D R
)		R S
:

 
base

 
(

 
rp

 
=>

 
rp

 
.

 
RoleId

 "
==

# %
roleId

& ,
&&

- /
rp

0 2
.

2 3

Permission

3 =
.

= >
Name

> B
==

C E
permissionName

F T
)

T U
{ 	

AddInclude 
( 
rp 
=> 
rp 
.  

Permission  *
)* +
;+ ,
} 	
} 
} ä
|C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\RolePermissions\PermissionsByRoleIdSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
.% &
RolePermissions& 5
{ 
public 

class ,
 PermissionsByRoleIdSpecification 1
:2 3
BaseSpecification4 E
<E F
RolePermissionF T
>T U
{ 
public ,
 PermissionsByRoleIdSpecification /
(/ 0
int0 3
roleId4 :
): ;
:		 
base		 
(		 
rp		 
=>		 
rp		 
.		 
RoleId		 "
==		# %
roleId		& ,
)		, -
{

 	

AddInclude 
( 
rp 
=> 
rp 
.  

Permission  *
)* +
;+ ,
} 	
} 
} ¯	
cC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\RoleByNameSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public

 

sealed

 
class

 #
RoleByNameSpecification

 /
:

0 1
BaseSpecification

2 C
<

C D
Role

D H
>

H I
{ 
public #
RoleByNameSpecification &
(& '
string' -
name. 2
,2 3
bool4 8
includeUsers9 E
=F G
falseH M
)M N
: 
base 
( 
r 
=> 
r 
. 
Name 
. 
ToLower &
(& '
)' (
==) +
name, 0
.0 1
ToLower1 8
(8 9
)9 :
): ;
{ 	
if 
( 
includeUsers 
) 
{ 

AddInclude 
( 
r 
=> 
r  !
.! "
Users" '
)' (
;( )
} 
} 	
} 
} ç
aC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\RoleByIdSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public

 

sealed

 
class

 !
RoleByIdSpecification

 -
:

. /
BaseSpecification

0 A
<

A B
Role

B F
>

F G
{ 
public !
RoleByIdSpecification $
($ %
int% (
id) +
)+ ,
: 
base 
( 
r 
=> 
r 
. 
Id 
== 
id  "
)" #
{ 	
} 	
} 
} €
kC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\PermissionsByGroupSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public		 

sealed		 
class		 +
PermissionsByGroupSpecification		 7
:		8 9
BaseSpecification		: K
<		K L

Permission		L V
>		V W
{

 
public +
PermissionsByGroupSpecification .
(. /
string/ 5
	groupName6 ?
)? @
: 
base 
( 
p 
=> 
p 
. 
Group 
==  "
	groupName# ,
), -
{ 	
ApplyOrderBy 
( 
p 
=> 
p 
.  
Name  $
)$ %
;% &
} 	
} 
} ¥
iC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\PermissionByNameSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public		 

sealed		 
class		 )
PermissionByNameSpecification		 5
:		6 7
BaseSpecification		8 I
<		I J

Permission		J T
>		T U
{

 
public )
PermissionByNameSpecification ,
(, -
string- 3
name4 8
)8 9
: 
base 
( 
p 
=> 
p 
. 
Name 
== !
name" &
)& '
{ 	
} 	
} 
} π
gC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\PermissionByIdSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public		 

sealed		 
class		 '
PermissionByIdSpecification		 3
:		4 5
BaseSpecification		6 G
<		G H

Permission		H R
>		R S
{

 
public '
PermissionByIdSpecification *
(* +
int+ .
permissionId/ ;
); <
: 
base 
( 
p 
=> 
p 
. 
Id 
== 
permissionId  ,
), -
{ 	
} 	
} 
} ∂
ZC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\ISpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public 

	interface 
ISpecification #
<# $
T$ %
>% &
{ 

Expression 
< 
Func 
< 
T 
, 
bool 
>  
>  !
?! "
Criteria# +
{, -
get. 1
;1 2
}3 4
List 
< 

Expression 
< 
Func 
< 
T 
, 
object  &
>& '
>' (
>( )
Includes* 2
{3 4
get5 8
;8 9
}: ;
List 
< 
string 
> 
IncludeStrings #
{$ %
get& )
;) *
}+ ,

Expression 
< 
Func 
< 
T 
, 
object !
>! "
>" #
?# $
OrderBy% ,
{- .
get/ 2
;2 3
}4 5

Expression$$ 
<$$ 
Func$$ 
<$$ 
T$$ 
,$$ 
object$$ !
>$$! "
>$$" #
?$$# $
OrderByDescending$$% 6
{$$7 8
get$$9 <
;$$< =
}$$> ?

Expression)) 
<)) 
Func)) 
<)) 
T)) 
,)) 
object)) !
>))! "
>))" #
?))# $
ThenBy))% +
{)), -
get)). 1
;))1 2
}))3 4

Expression.. 
<.. 
Func.. 
<.. 
T.. 
,.. 
object.. !
>..! "
>.." #
?..# $
ThenByDescending..% 5
{..6 7
get..8 ;
;..; <
}..= >
int33 
Take33 
{33 
get33 
;33 
}33 
int88 
Skip88 
{88 
get88 
;88 
}88 
bool== 
IsPagingEnabled== 
{== 
get== "
;==" #
}==$ %
boolBB 
IsAsNoTrackingBB 
{BB 
getBB !
;BB! "
}BB# $
boolGG 
IgnoreQueryFiltersGG 
{GG  !
getGG" %
;GG% &
}GG' (
}HH 
}II 
cC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\EntityByIdSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public 

sealed 
class #
EntityByIdSpecification /
</ 0
T0 1
>1 2
:3 4
BaseSpecification5 F
<F G
TG H
>H I
whereJ O
TP Q
:R S

BaseEntityT ^
{ 
public #
EntityByIdSpecification &
(& '
int' *
id+ -
,- .
params/ 5

Expression6 @
<@ A
FuncA E
<E F
TF G
,G H
objectI O
>O P
>P Q
[Q R
]R S
includesT \
)\ ]
: 
base 
( 
e 
=> 
e 
. 
Id 
== 
id  "
)" #
{ 	
if 
( 
includes 
!= 
null  
)  !
{ 
foreach 
( 
var 
include $
in% '
includes( 0
)0 1
{ 

AddInclude 
( 
include &
)& '
;' (
} 
} 
} 	
} 
} ûH
]C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\BaseSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public 

abstract 
class 
BaseSpecification +
<+ ,
T, -
>- .
:/ 0
ISpecification1 ?
<? @
T@ A
>A B
{ 
	protected 
BaseSpecification #
(# $
)$ %
{& '
}( )
	protected 
BaseSpecification #
(# $

Expression$ .
<. /
Func/ 3
<3 4
T4 5
,5 6
bool7 ;
>; <
>< =
criteria> F
)F G
{ 	
Criteria 
= 
criteria 
;  
} 	
public 

Expression 
< 
Func 
< 
T  
,  !
bool" &
>& '
>' (
?( )
Criteria* 2
{3 4
get5 8
;8 9
private: A
setB E
;E F
}G H
public   
List   
<   

Expression   
<   
Func   #
<  # $
T  $ %
,  % &
object  ' -
>  - .
>  . /
>  / 0
Includes  1 9
{  : ;
get  < ?
;  ? @
}  A B
=  C D
new  E H
List  I M
<  M N

Expression  N X
<  X Y
Func  Y ]
<  ] ^
T  ^ _
,  _ `
object  a g
>  g h
>  h i
>  i j
(  j k
)  k l
;  l m
public## 
List## 
<## 
string## 
>## 
IncludeStrings## *
{##+ ,
get##- 0
;##0 1
}##2 3
=##4 5
new##6 9
List##: >
<##> ?
string##? E
>##E F
(##F G
)##G H
;##H I
public&& 

Expression&& 
<&& 
Func&& 
<&& 
T&&  
,&&  !
object&&" (
>&&( )
>&&) *
?&&* +
OrderBy&&, 3
{&&4 5
get&&6 9
;&&9 :
private&&; B
set&&C F
;&&F G
}&&H I
public)) 

Expression)) 
<)) 
Func)) 
<)) 
T))  
,))  !
object))" (
>))( )
>))) *
?))* +
OrderByDescending)), =
{))> ?
get))@ C
;))C D
private))E L
set))M P
;))P Q
}))R S
public// 

Expression// 
<// 
Func// 
<// 
T//  
,//  !
object//" (
>//( )
>//) *
?//* +
ThenBy//, 2
{//3 4
get//5 8
;//8 9
private//: A
set//B E
;//E F
}//G H
public55 

Expression55 
<55 
Func55 
<55 
T55  
,55  !
object55" (
>55( )
>55) *
?55* +
ThenByDescending55, <
{55= >
get55? B
;55B C
private55D K
set55L O
;55O P
}55Q R
public88 
int88 
Take88 
{88 
get88 
;88 
private88 &
set88' *
;88* +
}88, -
public;; 
int;; 
Skip;; 
{;; 
get;; 
;;; 
private;; &
set;;' *
;;;* +
};;, -
public>> 
bool>> 
IsPagingEnabled>> #
{>>$ %
get>>& )
;>>) *
private>>+ 2
set>>3 6
;>>6 7
}>>8 9
publicAA 
boolAA 
IsAsNoTrackingAA "
{AA# $
getAA% (
;AA( )
privateAA* 1
setAA2 5
;AA5 6
}AA7 8
publicDD 
boolDD 
IgnoreQueryFiltersDD &
{DD' (
getDD) ,
;DD, -
privateDD. 5
setDD6 9
;DD9 :
}DD; <
	protectedJJ 
virtualJJ 
voidJJ 

AddIncludeJJ )
(JJ) *

ExpressionJJ* 4
<JJ4 5
FuncJJ5 9
<JJ9 :
TJJ: ;
,JJ; <
objectJJ= C
>JJC D
>JJD E
includeExpressionJJF W
)JJW X
{KK 	
IncludesLL 
.LL 
AddLL 
(LL 
includeExpressionLL *
)LL* +
;LL+ ,
}MM 	
	protectedTT 
virtualTT 
voidTT 

AddIncludeTT )
(TT) *
stringTT* 0
includeStringTT1 >
)TT> ?
{UU 	
IncludeStringsVV 
.VV 
AddVV 
(VV 
includeStringVV ,
)VV, -
;VV- .
}WW 	
	protected]] 
virtual]] 
void]] 
ApplyOrderBy]] +
(]]+ ,

Expression]], 6
<]]6 7
Func]]7 ;
<]]; <
T]]< =
,]]= >
object]]? E
>]]E F
>]]F G
orderByExpression]]H Y
)]]Y Z
{^^ 	
OrderBy__ 
=__ 
orderByExpression__ '
;__' (
OrderByDescending`` 
=`` 
null``  $
;``$ %
}aa 	
	protectedgg 
virtualgg 
voidgg "
ApplyOrderByDescendinggg 5
(gg5 6

Expressiongg6 @
<gg@ A
FuncggA E
<ggE F
TggF G
,ggG H
objectggI O
>ggO P
>ggP Q'
orderByDescendingExpressionggR m
)ggm n
{hh 	
OrderByDescendingii 
=ii '
orderByDescendingExpressionii  ;
;ii; <
OrderByjj 
=jj 
nulljj 
;jj 
}kk 	
	protectedss 
virtualss 
voidss 
ApplyThenByss *
(ss* +

Expressionss+ 5
<ss5 6
Funcss6 :
<ss: ;
Tss; <
,ss< =
objectss> D
>ssD E
>ssE F
thenByExpressionssG W
)ssW X
{tt 	
ifuu 
(uu 
OrderByuu 
==uu 
nulluu 
&&uu  "
OrderByDescendinguu# 4
==uu5 7
nulluu8 <
)uu< =
{vv 
throwww 
newww %
InvalidOperationExceptionww 3
(ww3 4
$str	ww4 í
)
wwí ì
;
wwì î
}xx 
ThenByyy 
=yy 
thenByExpressionyy %
;yy% &
ThenByDescendingzz 
=zz 
nullzz #
;zz# $
}{{ 	
	protected
ÉÉ 
virtual
ÉÉ 
void
ÉÉ #
ApplyThenByDescending
ÉÉ 4
(
ÉÉ4 5

Expression
ÉÉ5 ?
<
ÉÉ? @
Func
ÉÉ@ D
<
ÉÉD E
T
ÉÉE F
,
ÉÉF G
object
ÉÉH N
>
ÉÉN O
>
ÉÉO P(
thenByDescendingExpression
ÉÉQ k
)
ÉÉk l
{
ÑÑ 	
if
ÖÖ 
(
ÖÖ 
OrderBy
ÖÖ 
==
ÖÖ 
null
ÖÖ 
&&
ÖÖ  "
OrderByDescending
ÖÖ# 4
==
ÖÖ5 7
null
ÖÖ8 <
)
ÖÖ< =
{
ÜÜ 
throw
áá 
new
áá '
InvalidOperationException
áá 3
(
áá3 4
$stráá4 ú
)ááú ù
;ááù û
}
àà 
ThenByDescending
ââ 
=
ââ (
thenByDescendingExpression
ââ 9
;
ââ9 :
ThenBy
ää 
=
ää 
null
ää 
;
ää 
}
ãã 	
	protected
íí 
virtual
íí 
void
íí 
ApplyPaging
íí *
(
íí* +
int
íí+ .
skip
íí/ 3
,
íí3 4
int
íí5 8
take
íí9 =
)
íí= >
{
ìì 	
Skip
îî 
=
îî 
skip
îî 
;
îî 
Take
ïï 
=
ïï 
take
ïï 
;
ïï 
IsPagingEnabled
ññ 
=
ññ 
true
ññ "
;
ññ" #
}
óó 	
	protected
ùù 
virtual
ùù 
BaseSpecification
ùù +
<
ùù+ ,
T
ùù, -
>
ùù- .
AsNoTracking
ùù/ ;
(
ùù; <
)
ùù< =
{
ûû 	
IsAsNoTracking
üü 
=
üü 
true
üü !
;
üü! "
return
†† 
this
†† 
;
†† 
}
°° 	
	protected
ßß 
virtual
ßß 
BaseSpecification
ßß +
<
ßß+ ,
T
ßß, -
>
ßß- .%
WithIgnoredQueryFilters
ßß/ F
(
ßßF G
)
ßßG H
{
®® 	 
IgnoreQueryFilters
©© 
=
©©  
true
©©! %
;
©©% &
return
™™ 
this
™™ 
;
™™ 
}
´´ 	
}
¨¨ 
}≠≠ ö
\C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\AllSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public

 

sealed

 
class

 
AllSpecification

 (
<

( )
T

) *
>

* +
:

, -
BaseSpecification

. ?
<

? @
T

@ A
>

A B
{ 
public 
AllSpecification 
(  
)  !
:" #
base$ (
(( )
)) *
{ 	
} 	
} 
} ƒ
gC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Specifications\AllPermissionsSpecification.cs
	namespace 	
Stock
 
. 
Domain 
. 
Specifications %
{ 
public 

class '
AllPermissionsSpecification ,
:- .
BaseSpecification/ @
<@ A

PermissionA K
>K L
{		 
public

 '
AllPermissionsSpecification

 *
(

* +
)

+ ,
{ 	
} 	
} 
} Â
WC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Interfaces\IUserRepository.cs
	namespace 	
Stock
 
. 
Domain 
. 

Interfaces !
{ 
public 

	interface 
IUserRepository $
:% &
IRepository' 2
<2 3
User3 7
>7 8
{ 
Task 
< 
User 
? 
> 
GetBySicilAsync #
(# $
string$ *
sicil+ 0
)0 1
;1 2
} 
} Ó
SC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Interfaces\IUnitOfWork.cs
	namespace 	
Stock
 
. 
Domain 
. 

Interfaces !
{ 
public 

	interface 
IUnitOfWork  
:! "
IDisposable# .
{		 
IUserRepository

 
Users

 
{

 
get

  #
;

# $
}

% &
IRoleRepository 
Roles 
{ 
get  #
;# $
}% &!
IPermissionRepository 
Permissions )
{* +
get, /
;/ 0
}1 2
IProductRepository 
Products #
{$ %
get& )
;) *
}+ ,
ICategoryRepository 

Categories &
{' (
get) ,
;, -
}. /
IRepository 
< 
T 
> 
GetRepository $
<$ %
T% &
>& '
(' (
)( )
where* /
T0 1
:2 3

BaseEntity4 >
;> ?
Task 
< 
int 
> 
SaveChangesAsync "
(" #
CancellationToken# 4
cancellationToken5 F
=G H
defaultI P
)P Q
;Q R
Task"" !
BeginTransactionAsync"" "
(""" #
CancellationToken""# 4
cancellationToken""5 F
=""G H
default""I P
)""P Q
;""Q R
Task)) "
CommitTransactionAsync)) #
())# $
CancellationToken))$ 5
cancellationToken))6 G
=))H I
default))J Q
)))Q R
;))R S
Task// $
RollbackTransactionAsync// %
(//% &
CancellationToken//& 7
cancellationToken//8 I
=//J K
default//L S
)//S T
;//T U
}00 
}11 £
WC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Interfaces\IRoleRepository.cs
	namespace 	
Stock
 
. 
Domain 
. 

Interfaces !
{ 
public

 

	interface

 
IRoleRepository

 $
:

% &
IRepository

' 2
<

2 3
Role

3 7
>

7 8
{ 
} 
} ô$
SC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Interfaces\IRepository.cs
	namespace 	
Stock
 
. 
Domain 
. 

Interfaces !
{		 
public 

	interface 
IRepository  
<  !
T! "
>" #
where$ )
T* +
:, -

BaseEntity. 8
{ 
Task 
< 
T 
? 
> 
FirstOrDefaultAsync $
($ %
ISpecification% 3
<3 4
T4 5
>5 6
spec7 ;
,; <
CancellationToken= N
cancellationTokenO `
=a b
defaultc j
)j k
;k l
Task 
< 
IReadOnlyList 
< 
T 
> 
> 
	ListAsync (
(( )
ISpecification) 7
<7 8
T8 9
>9 :
spec; ?
,? @
CancellationTokenA R
cancellationTokenS d
=e f
defaultg n
)n o
;o p
Task'' 
<'' 
T'' 
?'' 
>'' 
GetByIdAsync'' 
('' 
int'' !
id''" $
,''$ %
CancellationToken''& 7
cancellationToken''8 I
=''J K
default''L S
)''S T
;''T U
Task// 
<// 
IEnumerable// 
<// 
T// 
>// 
>// 
ListEnumerableAsync// 0
(//0 1
ISpecification//1 ?
<//? @
T//@ A
>//A B
spec//C G
,//G H
CancellationToken//I Z
cancellationToken//[ l
=//m n
default//o v
)//v w
;//w x
Task77 
<77 
int77 
>77 

CountAsync77 
(77 
ISpecification77 +
<77+ ,
T77, -
>77- .
spec77/ 3
,773 4
CancellationToken775 F
cancellationToken77G X
=77Y Z
default77[ b
)77b c
;77c d
Task?? 
<?? 
bool?? 
>?? 
ExistsAsync?? 
(?? 

Expression?? )
<??) *
Func??* .
<??. /
T??/ 0
,??0 1
bool??2 6
>??6 7
>??7 8
	predicate??9 B
,??B C
CancellationToken??D U
cancellationToken??V g
=??h i
default??j q
)??q r
;??r s
TaskFF 
<FF 
TFF 
>FF 
AddAsyncFF 
(FF 
TFF 
entityFF !
,FF! "
CancellationTokenFF# 4
cancellationTokenFF5 F
=FFG H
defaultFFI P
)FFP Q
;FFQ R
TaskMM 
AddRangeAsyncMM 
(MM 
IEnumerableMM &
<MM& '
TMM' (
>MM( )
entitiesMM* 2
,MM2 3
CancellationTokenMM4 E
cancellationTokenMMF W
=MMX Y
defaultMMZ a
)MMa b
;MMb c
voidTT 
UpdateTT 
(TT 
TTT 
entityTT 
)TT 
;TT 
void[[ 
Delete[[ 
([[ 
T[[ 
entity[[ 
)[[ 
;[[ 
voidaa 
DeleteRangeaa 
(aa 
IEnumerableaa $
<aa$ %
Taa% &
>aa& '
entitiesaa( 0
)aa0 1
;aa1 2
Taskhh 
<hh 
IEnumerablehh 
<hh 
Thh 
>hh 
>hh 
GetAllAsynchh (
(hh( )
CancellationTokenhh) :
cancellationTokenhh; L
=hhM N
defaulthhO V
)hhV W
;hhW X
Taskoo 
UpdateAsyncoo 
(oo 
Too 
entityoo !
,oo! "
CancellationTokenoo# 4
cancellationTokenoo5 F
=ooG H
defaultooI P
)ooP Q
;ooQ R
Taskvv 
DeleteAsyncvv 
(vv 
Tvv 
entityvv !
,vv! "
CancellationTokenvv# 4
cancellationTokenvv5 F
=vvG H
defaultvvI P
)vvP Q
;vvQ R
Task}} 
<}} 
int}} 
>}} 
SaveChangesAsync}} "
(}}" #
CancellationToken}}# 4
cancellationToken}}5 F
=}}G H
default}}I P
)}}P Q
;}}Q R
}~~ 
} ˆ
ZC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Interfaces\IProductRepository.cs
	namespace 	
Stock
 
. 
Domain 
. 

Interfaces !
{ 
public 

	interface 
IProductRepository '
{ 
Task 
< 
Product 
> 
GetByIdAsync "
(" #
int# &
id' )
)) *
;* +
Task 
< 
IEnumerable 
< 
Product  
>  !
>! "
GetAllAsync# .
(. /
)/ 0
;0 1
Task		 
<		 
Product		 
>		 
AddAsync		 
(		 
Product		 &
product		' .
)		. /
;		/ 0
Task

 
UpdateAsync

 
(

 
Product

  
product

! (
)

( )
;

) *
Task 
DeleteAsync 
( 
Product  
product! (
)( )
;) *
} 
} µ
]C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Interfaces\IPermissionRepository.cs
	namespace 	
Stock
 
. 
Domain 
. 

Interfaces !
{ 
public		 

	interface		 !
IPermissionRepository		 *
:		+ ,
IRepository		- 8
<		8 9

Permission		9 C
>		C D
{

 
} 
} í
WC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Interfaces\IPasswordHasher.cs
	namespace 	
Stock
 
. 
Domain 
. 

Interfaces !
{ 
public 

	interface 
IPasswordHasher $
{ 
string 
HashPassword 
( 
string "
password# +
)+ ,
;, -
bool 
VerifyPassword 
( 
string "
password# +
,+ ,
string- 3
passwordHash4 @
)@ A
;A B
} 
} Ø
[C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Interfaces\ICategoryRepository.cs
	namespace 	
Stock
 
. 
Domain 
. 

Interfaces !
{ 
public 

	interface 
ICategoryRepository (
:) *
IRepository+ 6
<6 7
Category7 ?
>? @
{ 
} 
}		 Ö
[C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Exceptions\ValidationException.cs
	namespace 	
Stock
 
. 
Domain 
. 

Exceptions !
{ 
public		 

class		 
ValidationException		 $
:		% &
	Exception		' 0
{

 
public 
IDictionary 
< 
string !
,! "
string# )
[) *
]* +
>+ ,
Errors- 3
{4 5
get6 9
;9 :
}; <
public 
ValidationException "
(" #
)# $
:% &
base' +
(+ ,
$str, \
)\ ]
{ 	
Errors 
= 
new 

Dictionary #
<# $
string$ *
,* +
string, 2
[2 3
]3 4
>4 5
(5 6
)6 7
;7 8
} 	
public 
ValidationException "
(" #
string# )
message* 1
)1 2
:3 4
base5 9
(9 :
message: A
)A B
{ 	
Errors 
= 
new 

Dictionary #
<# $
string$ *
,* +
string, 2
[2 3
]3 4
>4 5
(5 6
)6 7
;7 8
} 	
public 
ValidationException "
(" #
IDictionary# .
<. /
string/ 5
,5 6
string7 =
[= >
]> ?
>? @
errorsA G
)G H
:I J
thisK O
(O P
)P Q
{ 	
Errors 
= 
errors 
; 
} 	
} 
} Æ
WC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Exceptions\DomainException.cs
	namespace 	
Stock
 
. 
Domain 
. 

Exceptions !
{ 
public 

class 
DomainException  
:! "
	Exception# ,
{ 
public 
DomainException 
( 
string %
message& -
)- .
:/ 0
base1 5
(5 6
message6 =
)= >
{ 	
}		 	
public 
DomainException 
( 
string %
message& -
,- .
	Exception/ 8
innerException9 G
)G H
:I J
baseK O
(O P
messageP W
,W X
innerExceptionY g
)g h
{ 	
} 	
} 
} ¨
YC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Exceptions\NotFoundException.cs
	namespace 	
Stock
 
. 
Domain 
. 

Exceptions !
{ 
public 

class 
NotFoundException "
:# $
	Exception% .
{		 
public

 
NotFoundException

  
(

  !
)

! "
:

# $
base

% )
(

) *
)

* +
{ 	
} 	
public 
NotFoundException  
(  !
string! '
message( /
)/ 0
:1 2
base3 7
(7 8
message8 ?
)? @
{ 	
} 	
public 
NotFoundException  
(  !
string! '
message( /
,/ 0
	Exception1 :
innerException; I
)I J
:K L
baseM Q
(Q R
messageR Y
,Y Z
innerException[ i
)i j
{ 	
} 	
public 
NotFoundException  
(  !
string! '

entityName( 2
,2 3
object4 :
key; >
)> ?
:@ A
baseB F
(F G
$"G I
$strI R
{R S

entityNameS ]
}] ^
$str^ b
{b c
keyc f
}f g
$strg w
"w x
)x y
{ 	
} 	
} 
} ‰
YC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Exceptions\ConflictException.cs
	namespace 	
Stock
 
. 
Domain 
. 

Exceptions !
{ 
public 

class 
ConflictException "
:# $
	Exception% .
{		 
public

 
ConflictException

  
(

  !
)

! "
:

# $
base

% )
(

) *
)

* +
{ 	
} 	
public 
ConflictException  
(  !
string! '
message( /
)/ 0
:1 2
base3 7
(7 8
message8 ?
)? @
{ 	
} 	
public 
ConflictException  
(  !
string! '
message( /
,/ 0
	Exception1 :
innerException; I
)I J
:K L
baseM Q
(Q R
messageR Y
,Y Z
innerException[ i
)i j
{ 	
} 	
} 
} ≥
JC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Entities\User.cs
	namespace 	
Stock
 
. 
Domain 
. 
Entities 
{ 
public 

class 
User 
: 

BaseEntity "
{		 
[

 	
Required

	 
]

 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Adi 
{ 
get 
;  
set! $
;$ %
}& '
=( )
string* 0
.0 1
Empty1 6
;6 7
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Soyadi 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
string- 3
.3 4
Empty4 9
;9 :
[ 	
Required	 
] 
public 
string 
PasswordHash "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
string3 9
.9 :
Empty: ?
;? @
public 
bool 
IsAdmin 
{ 
get !
;! "
set# &
;& '
}( )
public 
DateTime 
? 
LastLoginAt $
{% &
get' *
;* +
set, /
;/ 0
}1 2
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Sicil 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
int 
? 
RoleId 
{ 
get  
;  !
set" %
;% &
}' (
public 
virtual 
Role 
? 
Role !
{" #
get$ '
;' (
set) ,
;, -
}. /
public"" 
virtual"" 
ICollection"" "
<""" #
UserPermission""# 1
>""1 2
UserPermissions""3 B
{""C D
get""E H
;""H I
set""J M
;""M N
}""O P
=""Q R
new""S V
HashSet""W ^
<""^ _
UserPermission""_ m
>""m n
(""n o
)""o p
;""p q
public$$ 
User$$ 
($$ 
)$$ 
{%% 	
IsAdmin&& 
=&& 
false&& 
;&& 
UserPermissions'' 
='' 
new'' !
HashSet''" )
<'') *
UserPermission''* 8
>''8 9
(''9 :
)'': ;
;''; <
}(( 	
})) 
}** ø
JC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Entities\Role.cs
	namespace 	
Stock
 
. 
Domain 
. 
Entities 
{ 
public 

class 
Role 
: 

BaseEntity "
{ 
[		 	
Required			 
]		 
[

 	
StringLength

	 
(

 
$num

 
)

 
]

 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
=) *
string+ 1
.1 2
Empty2 7
;7 8
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public 
virtual 
ICollection "
<" #
User# '
>' (
Users) .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
== >
new? B
HashSetC J
<J K
UserK O
>O P
(P Q
)Q R
;R S
public 
virtual 
ICollection "
<" #
RolePermission# 1
>1 2
RolePermissions3 B
{C D
getE H
;H I
setJ M
;M N
}O P
=Q R
newS V
HashSetW ^
<^ _
RolePermission_ m
>m n
(n o
)o p
;p q
} 
} ´&
MC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Entities\Product.cs
	namespace 	
Stock
 
. 
Domain 
. 
Entities 
{ 
public 

class 
Product 
: 

BaseEntity %
{		 
public

 
ProductName

 
Name

 
{

  !
get

" %
;

% &
private

' .
set

/ 2
;

2 3
}

4 5
public 
ProductDescription !
Description" -
{. /
get0 3
;3 4
private5 <
set= @
;@ A
}B C
public 

StockLevel 

StockLevel $
{% &
get' *
;* +
private, 3
set4 7
;7 8
}9 :
public 
int 

CategoryId 
{ 
get  #
;# $
private% ,
set- 0
;0 1
}2 3
public 
Category 
Category  
{! "
get# &
;& '
private( /
set0 3
;3 4
}5 6
private 
Product 
( 
) 
{ 
} 
public 
static 
Product 
Create $
($ %
ProductName% 0
name1 5
,5 6
ProductDescription7 I
descriptionJ U
,U V

StockLevelW a

stockLevelb l
,l m
intn q

categoryIdr |
)| }
{ 	
if 
( 
name 
== 
null 
) 
throw #
new$ '
DomainException( 7
(7 8
$str8 V
)V W
;W X
if 
( 

stockLevel 
== 
null "
)" #
throw$ )
new* -
DomainException. =
(= >
$str> [
)[ \
;\ ]
if 
( 

categoryId 
<= 
$num 
)  
throw! &
new' *
DomainException+ :
(: ;
$str; P
)P Q
;Q R
return 
new 
Product 
{ 
Name 
= 
name 
, 
Description 
= 
description )
,) *

StockLevel 
= 

stockLevel '
,' (

CategoryId 
= 

categoryId '
} 
; 
}   	
public"" 
void"" 

UpdateName"" 
("" 
ProductName"" *
newName""+ 2
)""2 3
{## 	
if$$ 
($$ 
newName$$ 
==$$ 
null$$ 
)$$  
throw$$! &
new$$' *
DomainException$$+ :
($$: ;
$str$$; Y
)$$Y Z
;$$Z [
Name%% 
=%% 
newName%% 
;%% 
}&& 	
public(( 
void(( 
UpdateDescription(( %
(((% &
ProductDescription((& 8
newDescription((9 G
)((G H
{)) 	
Description** 
=** 
newDescription** (
;**( )
}++ 	
public-- 
void-- 
IncreaseStock-- !
(--! "
int--" %
amount--& ,
)--, -
{.. 	

StockLevel// 
=// 

StockLevel// #
.//# $
Increase//$ ,
(//, -
amount//- 3
)//3 4
;//4 5
}00 	
public22 
void22 
DecreaseStock22 !
(22! "
int22" %
amount22& ,
)22, -
{33 	

StockLevel44 
=44 

StockLevel44 #
.44# $
Decrease44$ ,
(44, -
amount44- 3
)443 4
;444 5
}55 	
public77 
void77 
ChangeCategory77 "
(77" #
int77# &
newCategoryId77' 4
)774 5
{88 	
if99 
(99 
newCategoryId99 
<=99  
$num99! "
)99" #
throw99$ )
new99* -
DomainException99. =
(99= >
$str99> S
)99S T
;99T U
if:: 
(:: 
newCategoryId:: 
==::  

CategoryId::! +
)::+ ,
return::- 3
;::3 4

CategoryId<< 
=<< 
newCategoryId<< &
;<<& '
Category== 
=== 
null== 
;== 
}>> 	
}?? 
}@@ ”	
`C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Entities\Permissions\UserPermission.cs
	namespace 	
Stock
 
. 
Domain 
. 
Entities 
.  
Permissions  +
{ 
public 

class 
UserPermission 
:  !

BaseEntity" ,
{ 
public 
int 
UserId 
{ 
get 
;  
set! $
;$ %
}& '
public		 
int		 
PermissionId		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		, -
public

 
bool

 
	IsGranted

 
{

 
get

  #
;

# $
set

% (
;

( )
}

* +
=

, -
true

. 2
;

2 3
public 
User 
User 
{ 
get 
; 
set  #
;# $
}% &
public 

Permission 

Permission $
{% &
get' *
;* +
set, /
;/ 0
}1 2
} 
} à
`C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Entities\Permissions\RolePermission.cs
	namespace 	
Stock
 
. 
Domain 
. 
Entities 
.  
Permissions  +
{ 
public 

class 
RolePermission 
:  !

BaseEntity" ,
{ 
public 
int 
RoleId 
{ 
get 
;  
set! $
;$ %
}& '
public		 
int		 
PermissionId		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		, -
public 
Role 
Role 
{ 
get 
; 
set  #
;# $
}% &
public 

Permission 

Permission $
{% &
get' *
;* +
set, /
;/ 0
}1 2
} 
} Ç
\C:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Entities\Permissions\Permission.cs
	namespace 	
Stock
 
. 
Domain 
. 
Entities 
.  
Permissions  +
{ 
public 

class 

Permission 
: 

BaseEntity (
{ 
public		 
string		 
Name		 
{		 
get		  
;		  !
set		" %
;		% &
}		' (
public

 
string

 
Description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

. /
public 
string 
ResourceType "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
ResourceName "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
Action 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
Group 
{ 
get !
;! "
set# &
;& '
}( )
public 
ICollection 
< 
RolePermission )
>) *
RolePermissions+ :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
ICollection 
< 
UserPermission )
>) *
UserPermissions+ :
{; <
get= @
;@ A
setB E
;E F
}G H
} 
} ô
NC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Entities\AuditLog.cs
	namespace 	
Stock
 
. 
Domain 
. 
Entities 
{ 
public 

class 
AuditLog 
: 

BaseEntity &
{ 
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
public		 
string		 
Action		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
=		+ ,
string		- 3
.		3 4
Empty		4 9
;		9 :
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 

EntityType  
{! "
get# &
;& '
set( +
;+ ,
}- .
=/ 0
string1 7
.7 8
Empty8 =
;= >
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
EntityId 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
public 
int 
? 
UserId 
{ 
get  
;  !
set" %
;% &
}' (
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
? 
Path 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
? 
Details 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
virtual 
User 
? 
User !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} √
PC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Entities\BaseEntity.cs
	namespace 	
Stock
 
. 
Domain 
. 
Entities 
{ 
public 

abstract 
class 

BaseEntity $
{ 
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
public 
DateTime 
	CreatedAt !
{" #
get$ '
;' (
set) ,
;, -
}. /
public		 
DateTime		 
?		 
	UpdatedAt		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		/ 0
public

 
string

 
?

 
	CreatedBy

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

- .
public 
string 
? 
	UpdatedBy  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
bool 
	IsDeleted 
{ 
get  #
;# $
set% (
;( )
}* +
	protected 

BaseEntity 
( 
) 
{ 	
	CreatedAt 
= 
DateTime  
.  !
UtcNow! '
;' (
	IsDeleted 
= 
false 
; 
} 	
} 
} Á
NC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Entities\Category.cs
	namespace 	
Stock
 
. 
Domain 
. 
Entities 
{ 
public 

class 
Category 
: 

BaseEntity &
{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}. /
public		 
ICollection		 
<		 
Product		 "
>		" #
Products		$ ,
{		- .
get		/ 2
;		2 3
set		4 7
;		7 8
}		9 :
}

 
} Å
QC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Entities\ActivityLog.cs
	namespace 	
Stock
 
. 
Domain 
. 
Entities 
{ 
public 

class 
ActivityLog 
: 

BaseEntity )
{ 
public 
int 
? 
UserId 
{ 
get  
;  !
set" %
;% &
}' (
[

 	
Required

	 
]

 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Username 
{  
get! $
;$ %
set& )
;) *
}+ ,
=- .
string/ 5
.5 6
Empty6 ;
;; <
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
ActivityType "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
string3 9
.9 :
Empty: ?
;? @
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
) 
] 
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public 
DateTime 
	Timestamp !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
? 
AdditionalInfo %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
bool 
IsSynchronized "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
virtual 
User 
? 
User !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
ActivityLog 
( 
) 
{   	
	Timestamp!! 
=!! 
DateTime!!  
.!!  !
UtcNow!!! '
;!!' (
IsSynchronized"" 
="" 
false"" "
;""" #
}## 	
}$$ 
}%% ®
OC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Common\ValueObject.cs
	namespace 	
Stock
 
. 
Domain 
. 
Common 
{ 
public 

abstract 
class 
ValueObject %
:& '

IEquatable( 2
<2 3
ValueObject3 >
>> ?
{ 
	protected		 
abstract		 
IEnumerable		 &
<		& '
object		' -
>		- .!
GetEqualityComponents		/ D
(		D E
)		E F
;		F G
public 
override 
bool 
Equals #
(# $
object$ *
obj+ .
). /
{ 	
if 
( 
obj 
== 
null 
|| 
obj "
." #
GetType# *
(* +
)+ ,
!=- /
GetType0 7
(7 8
)8 9
)9 :
{ 
return 
false 
; 
} 
var 
other 
= 
( 
ValueObject $
)$ %
obj% (
;( )
return !
GetEqualityComponents (
(( )
)) *
.* +
SequenceEqual+ 8
(8 9
other9 >
.> ?!
GetEqualityComponents? T
(T U
)U V
)V W
;W X
} 	
public 
override 
int 
GetHashCode '
(' (
)( )
{ 	
return !
GetEqualityComponents (
(( )
)) *
. 
Select 
( 
x 
=> 
x 
!= !
null" &
?' (
x) *
.* +
GetHashCode+ 6
(6 7
)7 8
:9 :
$num; <
)< =
. 
	Aggregate 
( 
( 
x 
, 
y  
)  !
=>" $
x% &
^' (
y) *
)* +
;+ ,
} 	
public 
bool 
Equals 
( 
ValueObject &
other' ,
), -
{ 	
return 
Equals 
( 
( 
object !
)! "
other" '
)' (
;( )
}   	
public"" 
static"" 
bool"" 
operator"" #
==""$ &
(""& '
ValueObject""' 2
left""3 7
,""7 8
ValueObject""9 D
right""E J
)""J K
{## 	
if$$ 
($$ 
ReferenceEquals$$ 
($$  
left$$  $
,$$$ %
null$$& *
)$$* +
^$$, -
ReferenceEquals$$. =
($$= >
right$$> C
,$$C D
null$$E I
)$$I J
)$$J K
{%% 
return&& 
false&& 
;&& 
}'' 
return(( 
ReferenceEquals(( "
(((" #
left((# '
,((' (
null(() -
)((- .
||((/ 1
left((2 6
.((6 7
Equals((7 =
(((= >
right((> C
)((C D
;((D E
})) 	
public++ 
static++ 
bool++ 
operator++ #
!=++$ &
(++& '
ValueObject++' 2
left++3 7
,++7 8
ValueObject++9 D
right++E J
)++J K
{,, 	
return-- 
!-- 
(-- 
left-- 
==-- 
right-- "
)--" #
;--# $
}.. 	
}// 
}00 å
JC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Common\Result.cs
	namespace 	
Stock
 
. 
Domain 
. 
Common 
; 
public 
class 
Result 
< 
T 
> 
{ 
public		 

bool		 
	IsSuccess		 
{		 
get		 
;		  
}		! "
public

 

T

 
Value

 
{

 
get

 
;

 
}

 
public 

string 
Error 
{ 
get 
; 
}  
	protected 
Result 
( 
bool 
	isSuccess #
,# $
T% &
value' ,
,, -
string. 4
error5 :
): ;
{ 
	IsSuccess 
= 
	isSuccess 
; 
Value 
= 
value 
; 
Error 
= 
error 
; 
} 
public 

static 
Result 
< 
T 
> 
Success #
(# $
T$ %
value& +
)+ ,
=>- /
new0 3
Result4 :
<: ;
T; <
>< =
(= >
true> B
,B C
valueD I
,I J
stringK Q
.Q R
EmptyR W
)W X
;X Y
public 

static 
Result 
< 
T 
> 
Failure #
(# $
string$ *
error+ 0
)0 1
=>2 4
new5 8
Result9 ?
<? @
T@ A
>A B
(B C
falseC H
,H I
defaultJ Q
(Q R
TR S
)S T
,T U
errorV [
)[ \
;\ ]
} 
public 
class 
Result 
{ 
public 

bool 
	IsSuccess 
{ 
get 
;  
}! "
public   

string   
Error   
{   
get   
;   
}    
	protected"" 
Result"" 
("" 
bool"" 
	isSuccess"" #
,""# $
string""% +
error"", 1
)""1 2
{## 
	IsSuccess$$ 
=$$ 
	isSuccess$$ 
;$$ 
Error%% 
=%% 
error%% 
;%% 
}&& 
public(( 

static(( 
Result(( 
Success((  
(((  !
)((! "
=>((# %
new((& )
Result((* 0
(((0 1
true((1 5
,((5 6
string((7 =
.((= >
Empty((> C
)((C D
;((D E
public** 

static** 
Result** 
Failure**  
(**  !
string**! '
error**( -
)**- .
=>**/ 1
new**2 5
Result**6 <
(**< =
false**= B
,**B C
error**D I
)**I J
;**J K
}++ Ú
CC:\Users\muham\OneDrive\Masa√ºst√º\Stock\src\Stock.Domain\Class1.cs
	namespace 	
Stock
 
. 
Domain 
; 
public 
class 
Class1 
{ 
} 