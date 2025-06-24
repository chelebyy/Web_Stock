�[
AC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Program.cs
public 
class 
Program 
{ 
public 

static 
async 
Task 
Main !
(! "
string" (
[( )
]) *
args+ /
)/ 0
{ 
var 
logger 
= 
NLog 
. 

LogManager $
.$ %
Setup% *
(* +
)+ ,
." #%
LoadConfigurationFromFile# <
(< =
$str= J
)J K
." #!
GetCurrentClassLogger# 8
(8 9
)9 :
;: ;
try 
{ 	
logger 
. 
Info 
( 
$str 2
)2 3
;3 4
var!! 
builder!! 
=!! 
WebApplication!! (
.!!( )
CreateBuilder!!) 6
(!!6 7
args!!7 ;
)!!; <
;!!< =
builder$$ 
.$$ 
Logging$$ 
.$$ 
ClearProviders$$ *
($$* +
)$$+ ,
;$$, -
builder%% 
.%% 
Host%% 
.%% 
UseNLog%%  
(%%  !
)%%! "
;%%" #
builder00 
.00 
Logging00 
.00 
ClearProviders00 *
(00* +
)00+ ,
;00, -
builder11 
.11 
Logging11 
.11 

AddConsole11 &
(11& '
)11' (
;11( )
builder22 
.22 
Logging22 
.22 
AddDebug22 $
(22$ %
)22% &
;22& '
builder33 
.33 
Logging33 
.33 
SetMinimumLevel33 +
(33+ ,
	Microsoft33, 5
.335 6

Extensions336 @
.33@ A
Logging33A H
.33H I
LogLevel33I Q
.33Q R
Information33R ]
)33] ^
;33^ _
builder66 
.66 
Services66 
.66 
	AddScoped66 &
<66& '-
!GlobalExceptionHandlingMiddleware66' H
>66H I
(66I J
)66J K
;66K L
builder99 
.99 
Services99 
.99 
AddApplication99 +
(99+ ,
)99, -
;99- .
builder:: 
.:: 
Services:: 
.:: 
AddInfrastructure:: .
(::. /
builder::/ 6
.::6 7
Configuration::7 D
)::D E
;::E F
builder== 
.== 
Services== 
.== 

AddMediatR== '
(==' (
cfg==( +
=>==, .
cfg>> 
.>> (
RegisterServicesFromAssembly>> 0
(>>0 1
typeof>>1 7
(>>7 8
Stock>>8 =
.>>= >
Application>>> I
.>>I J
DependencyInjection>>J ]
)>>] ^
.>>^ _
Assembly>>_ g
)>>g h
)>>h i
;>>i j
builderAA 
.AA 
ServicesAA 
.AA %
AddValidatorsFromAssemblyAA 6
(AA6 7
typeofAA7 =
(AA= >
StockAA> C
.AAC D
ApplicationAAD O
.AAO P
DependencyInjectionAAP c
)AAc d
.AAd e
AssemblyAAe m
)AAm n
;AAn o
builderDD 
.DD 
ServicesDD 
.DD -
!AddFluentValidationAutoValidationDD >
(DD> ?
)DD? @
;DD@ A
builderEE 
.EE 
ServicesEE 
.EE 1
%AddFluentValidationClientsideAdaptersEE B
(EEB C
)EEC D
;EED E
builderHH 
.HH 
ServicesHH 
.HH 
AddTransientHH )
(HH) *
typeofHH* 0
(HH0 1
IPipelineBehaviorHH1 B
<HHB C
,HHC D
>HHD E
)HHE F
,HHF G
typeofHHH N
(HHN O
ValidationBehaviorHHO a
<HHa b
,HHb c
>HHc d
)HHd e
)HHe f
;HHf g
builderKK 
.KK 
ServicesKK 
.KK 
AddAuthenticationKK .
(KK. /
optionsKK/ 6
=>KK7 9
{LL 
optionsMM 
.MM %
DefaultAuthenticateSchemeMM 1
=MM2 3
JwtBearerDefaultsMM4 E
.MME F 
AuthenticationSchemeMMF Z
;MMZ [
optionsNN 
.NN "
DefaultChallengeSchemeNN .
=NN/ 0
JwtBearerDefaultsNN1 B
.NNB C 
AuthenticationSchemeNNC W
;NNW X
}OO 
)OO 
.PP 
AddJwtBearerPP 
(PP 
optionsPP !
=>PP" $
{QQ 
optionsRR 
.RR %
TokenValidationParametersRR 1
=RR2 3
newRR4 7%
TokenValidationParametersRR8 Q
{SS 
ValidateIssuerTT "
=TT# $
trueTT% )
,TT) *
ValidateAudienceUU $
=UU% &
trueUU' +
,UU+ ,
ValidateLifetimeVV $
=VV% &
trueVV' +
,VV+ ,$
ValidateIssuerSigningKeyWW ,
=WW- .
trueWW/ 3
,WW3 4
ValidIssuerXX 
=XX  !
builderXX" )
.XX) *
ConfigurationXX* 7
[XX7 8
$strXX8 D
]XXD E
,XXE F
ValidAudienceYY !
=YY" #
builderYY$ +
.YY+ ,
ConfigurationYY, 9
[YY9 :
$strYY: H
]YYH I
,YYI J
IssuerSigningKeyZZ $
=ZZ% &
newZZ' * 
SymmetricSecurityKeyZZ+ ?
(ZZ? @
EncodingZZ@ H
.ZZH I
UTF8ZZI M
.ZZM N
GetBytesZZN V
(ZZV W
builderZZW ^
.ZZ^ _
ConfigurationZZ_ l
[ZZl m
$strZZm v
]ZZv w
)ZZw x
)ZZx y
}[[ 
;[[ 
}\\ 
)\\ 
;\\ 
builder__ 
.__ 
Services__ 
.__ 
AddControllers__ +
(__+ ,
)__, -
.`` 
AddJsonOptions`` 
(``  
options``  '
=>``( *
{aa 
optionscc 
.cc !
JsonSerializerOptionscc 1
.cc1 2
ReferenceHandlercc2 B
=ccC D
ReferenceHandlerccE U
.ccU V
PreserveccV ^
;cc^ _
optionsee 
.ee !
JsonSerializerOptionsee 1
.ee1 2"
DefaultIgnoreConditionee2 H
=eeI J
JsonIgnoreConditioneeK ^
.ee^ _
WhenWritingNullee_ n
;een o
optionsgg 
.gg !
JsonSerializerOptionsgg 1
.gg1 2

Convertersgg2 <
.gg< =
Addgg= @
(gg@ A
newggA D#
JsonStringEnumConverterggE \
(gg\ ]
)gg] ^
)gg^ _
;gg_ `
}hh 
)hh 
;hh 
builderkk 
.kk 
Serviceskk 
.kk #
AddEndpointsApiExplorerkk 4
(kk4 5
)kk5 6
;kk6 7
builderll 
.ll 
Servicesll 
.ll 
AddSwaggerGenll *
(ll* +
)ll+ ,
;ll, -
buildermm 
.mm 
Servicesmm 
.mm 
AddCorsmm $
(mm$ %
)mm% &
;mm& '
buildernn 
.nn 
Servicesnn 
.nn "
AddHttpContextAccessornn 3
(nn3 4
)nn4 5
;nn5 6
varpp 
apppp 
=pp 
builderpp 
.pp 
Buildpp #
(pp# $
)pp$ %
;pp% &
usingss 
(ss 
varss 
scopess 
=ss 
appss "
.ss" #
Servicesss# +
.ss+ ,
CreateScopess, 7
(ss7 8
)ss8 9
)ss9 :
{tt 
varuu 
servicesuu 
=uu 
scopeuu $
.uu$ %
ServiceProvideruu% 4
;uu4 5
varvv 
	appLoggervv 
=vv 
servicesvv  (
.vv( )
GetRequiredServicevv) ;
<vv; <
ILoggervv< C
<vvC D
ProgramvvD K
>vvK L
>vvL M
(vvM N
)vvN O
;vvO P
tryww 
{xx 
varyy 
contextyy 
=yy  !
servicesyy" *
.yy* +
GetRequiredServiceyy+ =
<yy= > 
ApplicationDbContextyy> R
>yyR S
(yyS T
)yyT U
;yyU V
	appLoggerzz 
.zz 
LogInformationzz ,
(zz, -
$strzz- r
)zzr s
;zzs t
	appLogger|| 
.|| 
LogInformation|| ,
(||, -
$str	||- �
)
||� �
;
||� �
	appLogger 
. 
LogInformation ,
(, -
$str- N
)N O
;O P
	appLogger
�� 
.
�� 
LogInformation
�� ,
(
��, -
$str
��- h
)
��h i
;
��i j
}
�� 
catch
�� 
(
�� 
	Exception
��  
ex
��! #
)
��# $
{
�� 
	appLogger
�� 
.
�� 
LogError
�� &
(
��& '
ex
��' )
,
��) *
$str
��+ h
)
��h i
;
��i j
}
�� 
}
�� 
app
�� 
.
�� 
UseMiddleware
�� 
<
�� /
!GlobalExceptionHandlingMiddleware
�� ?
>
��? @
(
��@ A
)
��A B
;
��B C
if
�� 
(
�� 
app
�� 
.
�� 
Environment
�� 
.
��  
IsDevelopment
��  -
(
��- .
)
��. /
)
��/ 0
{
�� 
app
�� 
.
�� 

UseSwagger
�� 
(
�� 
)
��  
;
��  !
app
�� 
.
�� 
UseSwaggerUI
��  
(
��  !
)
��! "
;
��" #
}
�� 
else
�� 
{
�� 
app
�� 
.
�� 
UseHsts
�� 
(
�� 
)
�� 
;
�� 
}
�� 
app
�� 
.
�� 
UseCors
�� 
(
�� 
$str
�� -
)
��- .
;
��. /
app
�� 
.
�� !
UseHttpsRedirection
�� #
(
��# $
)
��$ %
;
��% &
app
�� 
.
�� 
UseAuthentication
�� !
(
��! "
)
��" #
;
��# $
app
�� 
.
�� 
UseAuthorization
��  
(
��  !
)
��! "
;
��" #
app
�� 
.
�� 
MapControllers
�� 
(
�� 
)
��  
;
��  !
app
�� 
.
�� 
Run
�� 
(
�� 
)
�� 
;
�� 
}
�� 	
catch
�� 
(
�� 
	Exception
�� 
ex
�� 
)
�� 
{
�� 	
logger
�� 
.
�� 
Error
�� 
(
�� 
ex
�� 
,
�� 
$str
�� L
)
��L M
;
��M N
throw
�� 
;
�� 
}
�� 	
finally
�� 
{
�� 	

LogManager
�� 
.
�� 
Shutdown
�� 
(
��  
)
��  !
;
��! "
}
�� 	
}
�� 
}�� �+
fC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Middleware\GlobalExceptionHandlingMiddleware.cs
	namespace 	
Stock
 
. 
API 
. 

Middleware 
{ 
public 

class -
!GlobalExceptionHandlingMiddleware 2
:3 4
IMiddleware5 @
{ 
private 
readonly 
ILogger  
<  !-
!GlobalExceptionHandlingMiddleware! B
>B C
_loggerD K
;K L
private 
readonly 
IWebHostEnvironment ,
_env- 1
;1 2
public -
!GlobalExceptionHandlingMiddleware 0
(0 1
ILogger 
< -
!GlobalExceptionHandlingMiddleware 5
>5 6
logger7 =
,= >
IWebHostEnvironment 
env  #
)# $
{ 	
_logger 
= 
logger 
; 
_env   
=   
env   
;   
}!! 	
public(( 
async(( 
Task(( 
InvokeAsync(( %
(((% &
HttpContext((& 1
context((2 9
,((9 :
RequestDelegate((; J
next((K O
)((O P
{)) 	
try** 
{++ 
await,, 
next,, 
(,, 
context,, "
),," #
;,,# $
}-- 
catch.. 
(.. 
	Exception.. 
ex.. 
)..  
{// 
_logger00 
.00 
LogError00  
(00  !
ex00! #
,00# $
$str00% A
)00A B
;00B C
await11  
HandleExceptionAsync11 *
(11* +
context11+ 2
,112 3
ex114 6
)116 7
;117 8
}22 
}33 	
private55 
async55 
Task55  
HandleExceptionAsync55 /
(55/ 0
HttpContext550 ;
context55< C
,55C D
	Exception55E N
	exception55O X
)55X Y
{66 	
context77 
.77 
Response77 
.77 
ContentType77 (
=77) *
$str77+ =
;77= >
var99 
response99 
=99 
new99 
{:: 
Status;; 
=;; 
$str;;  
,;;  !
Message<< 
=<< "
GetUserFriendlyMessage<< 0
(<<0 1
	exception<<1 :
)<<: ;
,<<; <
DetailedMessage== 
===  !
_env==" &
.==& '
IsDevelopment==' 4
(==4 5
)==5 6
?==7 8
	exception==9 B
.==B C
ToString==C K
(==K L
)==L M
:==N O
null==P T
}>> 
;>> 
context@@ 
.@@ 
Response@@ 
.@@ 

StatusCode@@ '
=@@( )
GetStatusCode@@* 7
(@@7 8
	exception@@8 A
)@@A B
;@@B C
varBB 
resultBB 
=BB 
JsonSerializerBB '
.BB' (
	SerializeBB( 1
(BB1 2
responseBB2 :
)BB: ;
;BB; <
awaitCC 
contextCC 
.CC 
ResponseCC "
.CC" #

WriteAsyncCC# -
(CC- .
resultCC. 4
)CC4 5
;CC5 6
}DD 	
privateFF 
stringFF "
GetUserFriendlyMessageFF -
(FF- .
	ExceptionFF. 7
	exceptionFF8 A
)FFA B
{GG 	
returnHH 
	exceptionHH 
switchHH #
{II 
NotFoundExceptionJJ !
=>JJ" $
	exceptionJJ% .
.JJ. /
MessageJJ/ 6
,JJ6 7
FluentValidationKK  
.KK  !
ValidationExceptionKK! 4
=>KK5 7
ErrorMessagesKK8 E
.KKE F
ValidationErrorKKF U
,KKU V'
UnauthorizedAccessExceptionLL +
=>LL, .
ErrorMessagesLL/ <
.LL< =
UnauthorizedLL= I
,LLI J
_MM 
=>MM 
_envMM 
.MM 
IsDevelopmentMM '
(MM' (
)MM( )
?MM* +
	exceptionMM, 5
.MM5 6
MessageMM6 =
:MM> ?
ErrorMessagesMM@ M
.MMM N
GeneralServerErrorMMN `
}NN 
;NN 
}OO 	
privateQQ 
intQQ 
GetStatusCodeQQ !
(QQ! "
	ExceptionQQ" +
	exceptionQQ, 5
)QQ5 6
{RR 	
returnSS 
	exceptionSS 
switchSS #
{TT 
NotFoundExceptionUU !
=>UU" $
(UU% &
intUU& )
)UU) *
HttpStatusCodeUU* 8
.UU8 9
NotFoundUU9 A
,UUA B
FluentValidationVV  
.VV  !
ValidationExceptionVV! 4
=>VV5 7
(VV8 9
intVV9 <
)VV< =
HttpStatusCodeVV= K
.VVK L

BadRequestVVL V
,VVV W'
UnauthorizedAccessExceptionWW +
=>WW, .
(WW/ 0
intWW0 3
)WW3 4
HttpStatusCodeWW4 B
.WWB C
UnauthorizedWWC O
,WWO P
_XX 
=>XX 
(XX 
intXX 
)XX 
HttpStatusCodeXX (
.XX( )
InternalServerErrorXX) <
}YY 
;YY 
}ZZ 	
}[[ 
}\\ ��
UC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\UsersController.cs
	namespace 	
Stock
 
. 
API 
. 
Controllers 
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
ApiConstants 
. 
ApiBaseRoute $
+% &
$str' 6
)6 7
]7 8
public 

class 
UsersController  
:! "
ControllerBase# 1
{ 
private 
readonly 
	IMediator "
	_mediator# ,
;, -
private 
readonly 
ILogger  
<  !
UsersController! 0
>0 1
_logger2 9
;9 :
public%% 
UsersController%% 
(%% 
	IMediator%% (
mediator%%) 1
,%%1 2
ILogger%%3 :
<%%: ;
UsersController%%; J
>%%J K
logger%%L R
)%%R S
{&& 	
	_mediator'' 
='' 
mediator''  
;''  !
_logger(( 
=(( 
logger(( 
;(( 
})) 	
[22 	
HttpGet22	 
]22 
[33 	
	Authorize33	 
(33 
Roles33 
=33 
	RoleNames33 $
.33$ %
Admin33% *
)33* +
]33+ ,
[44 	 
ProducesResponseType44	 
(44 
typeof44 $
(44$ %
IEnumerable44% 0
<440 1
UserDto441 8
>448 9
)449 :
,44: ;
StatusCodes44< G
.44G H
Status200OK44H S
)44S T
]44T U
[55 	 
ProducesResponseType55	 
(55 
StatusCodes55 )
.55) *!
Status401Unauthorized55* ?
)55? @
]55@ A
[66 	 
ProducesResponseType66	 
(66 
typeof66 $
(66$ %
string66% +
)66+ ,
,66, -
StatusCodes66. 9
.669 :(
Status500InternalServerError66: V
)66V W
]66W X
public77 
async77 
Task77 
<77 
IActionResult77 '
>77' (
GetAll77) /
(77/ 0
)770 1
{88 	
try99 
{:: 
_logger;; 
.;; 
LogInformation;; &
(;;& '
LogMessages;;' 2
.;;2 3
FetchingAllUsers;;3 C
);;C D
;;;D E
var<< 
query<< 
=<< 
new<< 
GetAllUsersQuery<<  0
(<<0 1
)<<1 2
;<<2 3
var== 
result== 
=== 
await== "
	_mediator==# ,
.==, -
Send==- 1
(==1 2
query==2 7
)==7 8
;==8 9
_logger>> 
.>> 
LogInformation>> &
(>>& '
string>>' -
.>>- .
Format>>. 4
(>>4 5
LogMessages>>5 @
.>>@ A$
UsersFetchedSuccessfully>>A Y
,>>Y Z
result>>[ a
.>>a b
Count>>b g
(>>g h
)>>h i
)>>i j
)>>j k
;>>k l
return?? 
Ok?? 
(?? 
result??  
)??  !
;??! "
}@@ 
catchAA 
(AA 
	ExceptionAA 
exAA 
)AA  
{BB 
_loggerCC 
.CC 
LogErrorCC  
(CC  !
exCC! #
,CC# $
LogMessagesCC% 0
.CC0 1
ErrorFetchingUsersCC1 C
)CCC D
;CCD E
returnDD 

StatusCodeDD !
(DD! "
StatusCodesDD" -
.DD- .(
Status500InternalServerErrorDD. J
,DDJ K
newDDL O
{DDP Q
errorDDR W
=DDX Y
stringDDZ `
.DD` a
FormatDDa g
(DDg h
ErrorMessagesDDh u
.DDu v
GeneralServerError	DDv �
,
DD� �
ex
DD� �
.
DD� �
Message
DD� �
)
DD� �
}
DD� �
)
DD� �
;
DD� �
}EE 
}FF 	
[SS 	
HttpGetSS	 
(SS 
$strSS 
)SS 
]SS 
[TT 	
	AuthorizeTT	 
]TT 
[UU 	 
ProducesResponseTypeUU	 
(UU 
typeofUU $
(UU$ %
UserDtoUU% ,
)UU, -
,UU- .
StatusCodesUU/ :
.UU: ;
Status200OKUU; F
)UUF G
]UUG H
[VV 	 
ProducesResponseTypeVV	 
(VV 
typeofVV $
(VV$ %
stringVV% +
)VV+ ,
,VV, -
StatusCodesVV. 9
.VV9 :
Status404NotFoundVV: K
)VVK L
]VVL M
[WW 	 
ProducesResponseTypeWW	 
(WW 
StatusCodesWW )
.WW) *!
Status401UnauthorizedWW* ?
)WW? @
]WW@ A
[XX 	 
ProducesResponseTypeXX	 
(XX 
StatusCodesXX )
.XX) *
Status403ForbiddenXX* <
)XX< =
]XX= >
[YY 	 
ProducesResponseTypeYY	 
(YY 
typeofYY $
(YY$ %
stringYY% +
)YY+ ,
,YY, -
StatusCodesYY. 9
.YY9 :(
Status500InternalServerErrorYY: V
)YYV W
]YYW X
publicZZ 
asyncZZ 
TaskZZ 
<ZZ 
IActionResultZZ '
>ZZ' (
GetByIdZZ) 0
(ZZ0 1
intZZ1 4
idZZ5 7
)ZZ7 8
{[[ 	
try\\ 
{]] 
_logger^^ 
.^^ 
LogInformation^^ &
(^^& '
string^^' -
.^^- .
Format^^. 4
(^^4 5
LogMessages^^5 @
.^^@ A
FetchingUserById^^A Q
,^^Q R
id^^S U
)^^U V
)^^V W
;^^W X
var__ 
query__ 
=__ 
new__ 
GetUserByIdQuery__  0
{__1 2
Id__3 5
=__6 7
id__8 :
}__; <
;__< =
var`` 
result`` 
=`` 
await`` "
	_mediator``# ,
.``, -
Send``- 1
(``1 2
query``2 7
)``7 8
;``8 9
ifbb 
(bb 
resultbb 
==bb 
nullbb "
)bb" #
{cc 
_loggerdd 
.dd 

LogWarningdd &
(dd& '
stringdd' -
.dd- .
Formatdd. 4
(dd4 5
LogMessagesdd5 @
.dd@ A
UserNotFoundddA M
,ddM N
idddO Q
)ddQ R
)ddR S
;ddS T
returnee 
NotFoundee #
(ee# $
stringee$ *
.ee* +
Formatee+ 1
(ee1 2
ErrorMessagesee2 ?
.ee? @ 
UserNotFoundSpecificee@ T
,eeT U
ideeV X
)eeX Y
)eeY Z
;eeZ [
}ff 
_loggerhh 
.hh 
LogInformationhh &
(hh& '
stringhh' -
.hh- .
Formathh. 4
(hh4 5
LogMessageshh5 @
.hh@ A#
UserFetchedSuccessfullyhhA X
,hhX Y
idhhZ \
)hh\ ]
)hh] ^
;hh^ _
returnii 
Okii 
(ii 
resultii  
)ii  !
;ii! "
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
,mm# $
stringmm% +
.mm+ ,
Formatmm, 2
(mm2 3
LogMessagesmm3 >
.mm> ?
ErrorFetchingUsermm? P
,mmP Q
idmmR T
)mmT U
)mmU V
;mmV W
returnnn 

StatusCodenn !
(nn! "
StatusCodesnn" -
.nn- .(
Status500InternalServerErrornn. J
,nnJ K
newnnL O
{nnP Q
errornnR W
=nnX Y
stringnnZ `
.nn` a
Formatnna g
(nng h
ErrorMessagesnnh u
.nnu v
GeneralServerError	nnv �
,
nn� �
ex
nn� �
.
nn� �
Message
nn� �
)
nn� �
}
nn� �
)
nn� �
;
nn� �
}oo 
}pp 	
[|| 	
HttpPost||	 
]|| 
[}} 	
	Authorize}}	 
(}} 
Roles}} 
=}} 
	RoleNames}} $
.}}$ %
Admin}}% *
)}}* +
]}}+ ,
[~~ 	 
ProducesResponseType~~	 
(~~ 
typeof~~ $
(~~$ %
UserDto~~% ,
)~~, -
,~~- .
StatusCodes~~/ :
.~~: ;
Status201Created~~; K
)~~K L
]~~L M
[ 	 
ProducesResponseType	 
( 
typeof $
($ %
object% +
)+ ,
,, -
StatusCodes. 9
.9 :
Status400BadRequest: M
)M N
]N O
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :
Status409Conflict
��: K
)
��K L
]
��L M
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *#
Status401Unauthorized
��* ?
)
��? @
]
��@ A
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :*
Status500InternalServerError
��: V
)
��V W
]
��W X
public
�� 
async
�� 
Task
�� 
<
�� 
IActionResult
�� '
>
��' (
Create
��) /
(
��/ 0
[
��0 1
FromBody
��1 9
]
��9 :
CreateUserCommand
��; L
command
��M T
)
��T U
{
�� 	
try
�� 
{
�� 
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A
UserCreating
��A M
,
��M N
command
��O V
.
��V W
Sicil
��W \
)
��\ ]
)
��] ^
;
��^ _
var
�� 
result
�� 
=
�� 
await
�� "
	_mediator
��# ,
.
��, -
Send
��- 1
(
��1 2
command
��2 9
)
��9 :
;
��: ;
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A
UserCreated
��A L
,
��L M
result
��N T
.
��T U
Sicil
��U Z
,
��Z [
result
��\ b
.
��b c
Id
��c e
)
��e f
)
��f g
;
��g h
return
�� 
CreatedAtAction
�� &
(
��& '
nameof
��' -
(
��- .
GetById
��. 5
)
��5 6
,
��6 7
new
��8 ;
{
��< =
id
��> @
=
��A B
result
��C I
.
��I J
Id
��J L
}
��M N
,
��N O
result
��P V
)
��V W
;
��W X
}
�� 
catch
�� 
(
�� 
Stock
�� 
.
�� 
Domain
�� 
.
��  

Exceptions
��  *
.
��* +!
ValidationException
��+ >
ex
��? A
)
��A B
{
�� 
_logger
�� 
.
�� 

LogWarning
�� "
(
��" #
LogMessages
��# .
.
��. //
!ValidationErrorDuringUserCreation
��/ P
,
��P Q
ex
��R T
.
��T U
Message
��U \
)
��\ ]
;
��] ^
var
�� 
problemDetails
�� "
=
��# $
new
��% (&
ValidationProblemDetails
��) A
(
��A B
ex
��B D
.
��D E
Errors
��E K
.
�� 
GroupBy
�� 
(
�� 
kvp
��  
=>
��! #
kvp
��$ '
.
��' (
Key
��( +
)
��+ ,
.
�� 
ToDictionary
�� !
(
��! "
g
��" #
=>
��$ &
g
��' (
.
��( )
Key
��) ,
,
��, -
g
��. /
=>
��0 2
g
��3 4
.
��4 5

SelectMany
��5 ?
(
��? @
kvp
��@ C
=>
��D F
kvp
��G J
.
��J K
Value
��K P
)
��P Q
.
��Q R
ToArray
��R Y
(
��Y Z
)
��Z [
)
��[ \
)
��\ ]
;
��] ^
return
�� 

BadRequest
�� !
(
��! "
problemDetails
��" 0
)
��0 1
;
��1 2
}
�� 
catch
�� 
(
�� 
ConflictException
�� $
ex
��% '
)
��' (
{
�� 
_logger
�� 
.
�� 

LogWarning
�� "
(
��" #
LogMessages
��# .
.
��. /(
ConflictDuringUserCreation
��/ I
,
��I J
command
��K R
.
��R S
Sicil
��S X
,
��X Y
ex
��Z \
.
��\ ]
Message
��] d
)
��d e
;
��e f
return
�� 
Conflict
�� 
(
��  
new
��  #
{
��$ %
error
��& +
=
��, -
ex
��. 0
.
��0 1
Message
��1 8
}
��9 :
)
��: ;
;
��; <
}
�� 
catch
�� 
(
�� 
NotFoundException
�� $
ex
��% '
)
��' (
{
�� 
_logger
�� 
.
�� 

LogWarning
�� "
(
��" #
LogMessages
��# .
.
��. /(
NotFoundDuringUserCreation
��/ I
,
��I J
ex
��K M
.
��M N
Message
��N U
)
��U V
;
��V W
return
�� 
NotFound
�� 
(
��  
new
��  #
{
��$ %
error
��& +
=
��, -
ex
��. 0
.
��0 1
Message
��1 8
}
��9 :
)
��: ;
;
��; <
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
��0 1%
ErrorDuringUserCreation
��1 H
)
��H I
;
��I J
return
�� 

StatusCode
�� !
(
��! "
StatusCodes
��" -
.
��- .*
Status500InternalServerError
��. J
,
��J K
new
��L O
{
��P Q
error
��R W
=
��X Y
ErrorMessages
��Z g
.
��g h
UserCreationError
��h y
}
��z {
)
��{ |
;
��| }
}
�� 
}
�� 	
[
�� 	
HttpPut
��	 
(
�� 
$str
�� 
)
�� 
]
�� 
[
�� 	
	Authorize
��	 
(
�� 
Roles
�� 
=
�� 
	RoleNames
�� $
.
��$ %
Admin
��% *
)
��* +
]
��+ ,
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
UserDto
��% ,
)
��, -
,
��- .
StatusCodes
��/ :
.
��: ;
Status200OK
��; F
)
��F G
]
��G H
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :!
Status400BadRequest
��: M
)
��M N
]
��N O
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :
Status409Conflict
��: K
)
��K L
]
��L M
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *#
Status401Unauthorized
��* ?
)
��? @
]
��@ A
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :
Status404NotFound
��: K
)
��K L
]
��L M
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :*
Status500InternalServerError
��: V
)
��V W
]
��W X
public
�� 
async
�� 
Task
�� 
<
�� 
IActionResult
�� '
>
��' (
Update
��) /
(
��/ 0
int
��0 3
id
��4 6
,
��6 7
[
��8 9
FromBody
��9 A
]
��A B
UpdateUserCommand
��C T
command
��U \
)
��\ ]
{
�� 	
try
�� 
{
�� 
if
�� 
(
�� 
id
�� 
!=
�� 
command
�� !
.
��! "
Id
��" $
)
��$ %
{
�� 
_logger
�� 
.
�� 

LogWarning
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A!
RouteBodyIdMismatch
��A T
,
��T U
id
��V X
,
��X Y
command
��Z a
.
��a b
Id
��b d
)
��d e
)
��e f
;
��f g
return
�� 

BadRequest
�� %
(
��% &
new
��& )
{
��* +
error
��, 1
=
��2 3
ErrorMessages
��4 A
.
��A B!
RouteBodyIdMismatch
��B U
}
��V W
)
��W X
;
��X Y
}
�� 
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A
UserUpdating
��A M
,
��M N
id
��O Q
)
��Q R
)
��R S
;
��S T
var
�� 
result
�� 
=
�� 
await
�� "
	_mediator
��# ,
.
��, -
Send
��- 1
(
��1 2
command
��2 9
)
��9 :
;
��: ;
if
�� 
(
�� 
result
�� 
==
�� 
null
�� "
)
��" #
{
�� 
_logger
�� 
.
�� 

LogWarning
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A#
UserNotFoundForUpdate
��A V
,
��V W
id
��X Z
)
��Z [
)
��[ \
;
��\ ]
return
�� 
NotFound
�� #
(
��# $
new
��$ '
{
��( )
error
��* /
=
��0 1
string
��2 8
.
��8 9
Format
��9 ?
(
��? @
ErrorMessages
��@ M
.
��M N"
UserNotFoundSpecific
��N b
,
��b c
id
��d f
)
��f g
}
��h i
)
��i j
;
��j k
}
�� 
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A
UserUpdated
��A L
,
��L M
result
��N T
.
��T U
Id
��U W
)
��W X
)
��X Y
;
��Y Z
return
�� 
Ok
�� 
(
�� 
result
��  
)
��  !
;
��! "
}
�� 
catch
�� 
(
�� 
Stock
�� 
.
�� 
Domain
�� 
.
��  

Exceptions
��  *
.
��* +!
ValidationException
��+ >
ex
��? A
)
��A B
{
�� 
_logger
�� 
.
�� 

LogWarning
�� "
(
��" #
LogMessages
��# .
.
��. /-
ValidationErrorDuringUserUpdate
��/ N
,
��N O
ex
��P R
.
��R S
Message
��S Z
)
��Z [
;
��[ \
var
�� 
problemDetails
�� "
=
��# $
new
��% (&
ValidationProblemDetails
��) A
(
��A B
ex
��B D
.
��D E
Errors
��E K
.
�� 
GroupBy
�� 
(
�� 
kvp
��  
=>
��! #
kvp
��$ '
.
��' (
Key
��( +
)
��+ ,
.
�� 
ToDictionary
�� !
(
��! "
g
��" #
=>
��$ &
g
��' (
.
��( )
Key
��) ,
,
��, -
g
��. /
=>
��0 2
g
��3 4
.
��4 5

SelectMany
��5 ?
(
��? @
kvp
��@ C
=>
��D F
kvp
��G J
.
��J K
Value
��K P
)
��P Q
.
��Q R
ToArray
��R Y
(
��Y Z
)
��Z [
)
��[ \
)
��\ ]
;
��] ^
return
�� 

BadRequest
�� !
(
��! "
problemDetails
��" 0
)
��0 1
;
��1 2
}
�� 
catch
�� 
(
�� 
ConflictException
�� $
ex
��% '
)
��' (
{
�� 
_logger
�� 
.
�� 

LogWarning
�� "
(
��" #
LogMessages
��# .
.
��. /&
ConflictDuringUserUpdate
��/ G
,
��G H
id
��I K
,
��K L
ex
��M O
.
��O P
Message
��P W
)
��W X
;
��X Y
return
�� 
Conflict
�� 
(
��  
new
��  #
{
��$ %
error
��& +
=
��, -
ex
��. 0
.
��0 1
Message
��1 8
}
��9 :
)
��: ;
;
��; <
}
�� 
catch
�� 
(
�� 
NotFoundException
�� $
ex
��% '
)
��' (
{
�� 
_logger
�� 
.
�� 

LogWarning
�� "
(
��" #
LogMessages
��# .
.
��. /&
NotFoundDuringUserUpdate
��/ G
,
��G H
id
��I K
,
��K L
ex
��M O
.
��O P
Message
��P W
)
��W X
;
��X Y
return
�� 
NotFound
�� 
(
��  
new
��  #
{
��$ %
error
��& +
=
��, -
ex
��. 0
.
��0 1
Message
��1 8
}
��9 :
)
��: ;
;
��; <
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
��# $
string
��% +
.
��+ ,
Format
��, 2
(
��2 3
LogMessages
��3 >
.
��> ?
ErrorUpdatingUser
��? P
,
��P Q
id
��R T
)
��T U
)
��U V
;
��V W
return
�� 

StatusCode
�� !
(
��! "
StatusCodes
��" -
.
��- .*
Status500InternalServerError
��. J
,
��J K
new
��L O
{
��P Q
error
��R W
=
��X Y
ErrorMessages
��Z g
.
��g h
UserUpdateError
��h w
}
��x y
)
��y z
;
��z {
}
�� 
}
�� 	
[
�� 	

HttpDelete
��	 
(
�� 
$str
�� 
)
�� 
]
�� 
[
�� 	
	Authorize
��	 
(
�� 
Roles
�� 
=
�� 
	RoleNames
�� $
.
��$ %
Admin
��% *
)
��* +
]
��+ ,
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) * 
Status204NoContent
��* <
)
��< =
]
��= >
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *#
Status401Unauthorized
��* ?
)
��? @
]
��@ A
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :
Status404NotFound
��: K
)
��K L
]
��L M
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :*
Status500InternalServerError
��: V
)
��V W
]
��W X
public
�� 
async
�� 
Task
�� 
<
�� 
IActionResult
�� '
>
��' (
Delete
��) /
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
�� 
LogInformation
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A
UserDeleting
��A M
,
��M N
id
��O Q
)
��Q R
)
��R S
;
��S T
var
�� 
command
�� 
=
�� 
new
�� !
DeleteUserCommand
��" 3
{
��4 5
Id
��6 8
=
��9 :
id
��; =
}
��> ?
;
��? @
await
�� 
	_mediator
�� 
.
��  
Send
��  $
(
��$ %
command
��% ,
)
��, -
;
��- .
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A
UserDeleted
��A L
,
��L M
id
��N P
)
��P Q
)
��Q R
;
��R S
return
�� 
	NoContent
��  
(
��  !
)
��! "
;
��" #
}
�� 
catch
�� 
(
�� 
NotFoundException
�� $
ex
��% '
)
��' (
{
�� 
_logger
�� 
.
�� 

LogWarning
�� "
(
��" #
LogMessages
��# .
.
��. /&
NotFoundDuringUserDelete
��/ G
,
��G H
id
��I K
,
��K L
ex
��M O
.
��O P
Message
��P W
)
��W X
;
��X Y
return
�� 
NotFound
�� 
(
��  
new
��  #
{
��$ %
error
��& +
=
��, -
ex
��. 0
.
��0 1
Message
��1 8
}
��9 :
)
��: ;
;
��; <
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
��# $
string
��% +
.
��+ ,
Format
��, 2
(
��2 3
LogMessages
��3 >
.
��> ?#
ErrorDuringUserDelete
��? T
,
��T U
id
��V X
)
��X Y
)
��Y Z
;
��Z [
return
�� 

StatusCode
�� !
(
��! "
StatusCodes
��" -
.
��- .*
Status500InternalServerError
��. J
,
��J K
new
��L O
{
��P Q
error
��R W
=
��X Y
ErrorMessages
��Z g
.
��g h
UserDeleteError
��h w
}
��x y
)
��y z
;
��z {
}
�� 
}
�� 	
}
�� 
}�� �S
^C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\UserPermissionController.cs
	namespace

 	
Stock


 
.

 
API

 
.

 
Controllers

 
{ 
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class $
UserPermissionController )
:* +
ControllerBase, :
{ 
private 
readonly "
IUserPermissionService /"
_userPermissionService0 F
;F G
private 
readonly 
ILogger  
<  !$
UserPermissionController! 9
>9 :
_logger; B
;B C
public $
UserPermissionController '
(' ("
IUserPermissionService "!
userPermissionService# 8
,8 9
ILogger 
< $
UserPermissionController ,
>, -
logger. 4
)4 5
{ 	"
_userPermissionService "
=# $!
userPermissionService% :
;: ;
_logger 
= 
logger 
; 
} 	
[ 	
HttpGet	 
( 
$str  
)  !
]! "
[ 	
	Authorize	 
( 
Policy 
= 
$str 3
)3 4
]4 5
public 
async 
Task 
< 
ActionResult &
<& '
List' +
<+ ,
PermissionDto, 9
>9 :
>: ;
>; <
GetUserPermissions= O
(O P
intP S
userIdT Z
)Z [
{ 	
try   
{!! 
var"" 
permissions"" 
=""  !
await""" '"
_userPermissionService""( >
.""> ?#
GetUserPermissionsAsync""? V
(""V W
userId""W ]
)""] ^
;""^ _
return## 
Ok## 
(## 
permissions## %
)##% &
;##& '
}$$ 
catch%% 
(%% 
	Exception%% 
ex%% 
)%%  
{&& 
_logger'' 
.'' 
LogError''  
(''  !
ex''! #
,''# $
$str''% g
,''g h
userId''i o
)''o p
;''p q
return(( 

StatusCode(( !
(((! "
$num((" %
,((% &
$str((' V
)((V W
;((W X
})) 
}** 	
[,, 	
HttpGet,,	 
(,, 
$str,, '
),,' (
],,( )
[-- 	
	Authorize--	 
(-- 
Policy-- 
=-- 
$str-- 3
)--3 4
]--4 5
public.. 
async.. 
Task.. 
<.. 
ActionResult.. &
<..& '
List..' +
<..+ ,
UserPermissionDto.., =
>..= >
>..> ?
>..? @$
GetUserCustomPermissions..A Y
(..Y Z
int..Z ]
userId..^ d
)..d e
{// 	
try00 
{11 
var22 
customPermissions22 %
=22& '
await22( -"
_userPermissionService22. D
.22D E)
GetUserCustomPermissionsAsync22E b
(22b c
userId22c i
)22i j
;22j k
return33 
Ok33 
(33 
customPermissions33 +
)33+ ,
;33, -
}44 
catch55 
(55 
	Exception55 
ex55 
)55  
{66 
_logger77 
.77 
LogError77  
(77  !
ex77! #
,77# $
$str77% l
,77l m
userId77n t
)77t u
;77u v
return88 

StatusCode88 !
(88! "
$num88" %
,88% &
$str88' [
)88[ \
;88\ ]
}99 
}:: 	
[<< 	
HttpPost<<	 
(<< 
$str<< ;
)<<; <
]<<< =
[== 	
	Authorize==	 
(== 
Policy== 
=== 
$str== 3
)==3 4
]==4 5
public>> 
async>> 
Task>> 
<>> 
ActionResult>> &
>>>& '"
AssignPermissionToUser>>( >
(>>> ?
int>>? B
userId>>C I
,>>I J
int>>K N
permissionId>>O [
,>>[ \
[>>] ^
	FromQuery>>^ g
]>>g h
bool>>i m
	isGranted>>n w
=>>x y
true>>z ~
)>>~ 
{?? 	
try@@ 
{AA 
varBB 
resultBB 
=BB 
awaitBB ""
_userPermissionServiceBB# 9
.BB9 :'
AssignPermissionToUserAsyncBB: U
(BBU V
userIdBBV \
,BB\ ]
permissionIdBB^ j
,BBj k
	isGrantedBBl u
)BBu v
;BBv w
ifCC 
(CC 
!CC 
resultCC 
)CC 
{DD 
returnEE 

BadRequestEE %
(EE% &
$strEE& C
)EEC D
;EED E
}FF 
returnGG 
OkGG 
(GG 
)GG 
;GG 
}HH 
catchII 
(II 
	ExceptionII 
exII 
)II  
{JJ 
_loggerKK 
.KK 
LogErrorKK  
(KK  !
exKK! #
,KK# $
$strKK% ~
,KK~ 
userId
KK� �
,
KK� �
permissionId
KK� �
)
KK� �
;
KK� �
returnLL 

StatusCodeLL !
(LL! "
$numLL" %
,LL% &
$strLL' T
)LLT U
;LLU V
}MM 
}NN 	
[PP 	

HttpDeletePP	 
(PP 
$strPP =
)PP= >
]PP> ?
[QQ 	
	AuthorizeQQ	 
(QQ 
PolicyQQ 
=QQ 
$strQQ 3
)QQ3 4
]QQ4 5
publicRR 
asyncRR 
TaskRR 
<RR 
ActionResultRR &
>RR& ' 
RemoveUserPermissionRR( <
(RR< =
intRR= @
userIdRRA G
,RRG H
intRRI L
permissionIdRRM Y
)RRY Z
{SS 	
tryTT 
{UU 
varVV 
resultVV 
=VV 
awaitVV ""
_userPermissionServiceVV# 9
.VV9 :%
RemoveUserPermissionAsyncVV: S
(VVS T
userIdVVT Z
,VVZ [
permissionIdVV\ h
)VVh i
;VVi j
ifWW 
(WW 
!WW 
resultWW 
)WW 
{XX 
returnYY 

BadRequestYY %
(YY% &
$strYY& H
)YYH I
;YYI J
}ZZ 
return[[ 
Ok[[ 
([[ 
)[[ 
;[[ 
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
,__# $
$str	__% �
,
__� �
userId
__� �
,
__� �
permissionId
__� �
)
__� �
;
__� �
return`` 

StatusCode`` !
(``! "
$num``" %
,``% &
$str``' Y
)``Y Z
;``Z [
}aa 
}bb 	
[dd 	
HttpPostdd	 
(dd 
$strdd '
)dd' (
]dd( )
[ee 	
	Authorizeee	 
(ee 
Policyee 
=ee 
$stree 3
)ee3 4
]ee4 5
publicff 
asyncff 
Taskff 
<ff 
ActionResultff &
>ff& '&
ResetUserToRolePermissionsff( B
(ffB C
intffC F
userIdffG M
)ffM N
{gg 	
tryhh 
{ii 
varjj 
resultjj 
=jj 
awaitjj ""
_userPermissionServicejj# 9
.jj9 :+
ResetUserToRolePermissionsAsyncjj: Y
(jjY Z
userIdjjZ `
)jj` a
;jja b
ifkk 
(kk 
!kk 
resultkk 
)kk 
{ll 
returnmm 

BadRequestmm %
(mm% &
$strmm& X
)mmX Y
;mmY Z
}nn 
returnoo 
Okoo 
(oo 
)oo 
;oo 
}pp 
catchqq 
(qq 
	Exceptionqq 
exqq 
)qq  
{rr 
_loggerss 
.ss 
LogErrorss  
(ss  !
exss! #
,ss# $
$strss% z
,ssz {
userId	ss| �
)
ss� �
;
ss� �
returntt 

StatusCodett !
(tt! "
$numtt" %
,tt% &
$strtt' i
)tti j
;ttj k
}uu 
}vv 	
[xx 	
HttpGetxx	 
(xx 
$strxx 7
)xx7 8
]xx8 9
publicyy 
asyncyy 
Taskyy 
<yy 
ActionResultyy &
<yy& '
boolyy' +
>yy+ ,
>yy, -
HasPermissionyy. ;
(yy; <
intyy< ?
userIdyy@ F
,yyF G
stringyyH N
permissionNameyyO ]
)yy] ^
{zz 	
try{{ 
{|| 
var}} 
hasPermission}} !
=}}" #
await}}$ )"
_userPermissionService}}* @
.}}@ A
HasPermissionAsync}}A S
(}}S T
userId}}T Z
,}}Z [
permissionName}}\ j
)}}j k
;}}k l
return~~ 
Ok~~ 
(~~ 
hasPermission~~ '
)~~' (
;~~( )
} 
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
��# $
$str
��% 
,�� �
userId��� �
,��� �
permissionName��� �
)��� �
;��� �
return
�� 

StatusCode
�� !
(
��! "
$num
��" %
,
��% &
$str
��' R
)
��R S
;
��S T
}
�� 
}
�� 	
}
�� 
}�� V
TC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\TestController.csU
SC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\SqlController.cs�J
TC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\RoleController.cs
	namespace 	
Stock
 
. 
API 
. 
Controllers 
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
ApiConstants 
. 
ApiBaseRoute $
+% &
$str' 6
)6 7
]7 8
public 

class 
RoleController 
:  !
ControllerBase" 0
{ 
private 
readonly 
	IMediator "
	_mediator# ,
;, -
public"" 
RoleController"" 
("" 
	IMediator"" '
mediator""( 0
)""0 1
{## 	
	_mediator$$ 
=$$ 
mediator$$  
;$$  !
}%% 	
[-- 	
HttpGet--	 
]-- 
[.. 	 
ProducesResponseType..	 
(.. 
typeof.. $
(..$ %
IEnumerable..% 0
<..0 1
RoleDto..1 8
>..8 9
)..9 :
,..: ;
StatusCodes..< G
...G H
Status200OK..H S
)..S T
]..T U
[// 	 
ProducesResponseType//	 
(// 
typeof// $
(//$ %
string//% +
)//+ ,
,//, -
StatusCodes//. 9
.//9 :(
Status500InternalServerError//: V
)//V W
]//W X
public00 
async00 
Task00 
<00 
ActionResult00 &
<00& '
IEnumerable00' 2
<002 3
RoleDto003 :
>00: ;
>00; <
>00< =
GetRoles00> F
(00F G
)00G H
{11 	
try22 
{33 
var44 
query44 
=44 
new44 
GetAllRolesQuery44  0
(440 1
)441 2
;442 3
var55 
result55 
=55 
await55 "
	_mediator55# ,
.55, -
Send55- 1
(551 2
query552 7
)557 8
;558 9
return66 
Ok66 
(66 
result66  
)66  !
;66! "
}77 
catch88 
(88 
	Exception88 
ex88 
)88  
{99 
return:: 

StatusCode:: !
(::! "
StatusCodes::" -
.::- .(
Status500InternalServerError::. J
,::J K
string::L R
.::R S
Format::S Y
(::Y Z
ErrorMessages::Z g
.::g h
GeneralServerError::h z
,::z {
ex::| ~
.::~ 
Message	:: �
)
::� �
)
::� �
;
::� �
};; 
}<< 	
[FF 	
HttpGetFF	 
(FF 
$strFF 
)FF 
]FF 
[GG 	 
ProducesResponseTypeGG	 
(GG 
typeofGG $
(GG$ %
RoleDtoGG% ,
)GG, -
,GG- .
StatusCodesGG/ :
.GG: ;
Status200OKGG; F
)GGF G
]GGG H
[HH 	 
ProducesResponseTypeHH	 
(HH 
StatusCodesHH )
.HH) *
Status404NotFoundHH* ;
)HH; <
]HH< =
[II 	 
ProducesResponseTypeII	 
(II 
typeofII $
(II$ %
stringII% +
)II+ ,
,II, -
StatusCodesII. 9
.II9 :(
Status500InternalServerErrorII: V
)IIV W
]IIW X
publicJJ 
asyncJJ 
TaskJJ 
<JJ 
ActionResultJJ &
<JJ& '
RoleDtoJJ' .
>JJ. /
>JJ/ 0
GetRoleJJ1 8
(JJ8 9
intJJ9 <
idJJ= ?
)JJ? @
{KK 	
tryLL 
{MM 
varNN 
queryNN 
=NN 
newNN 
GetRoleByIdQueryNN  0
(NN0 1
idNN1 3
)NN3 4
;NN4 5
varOO 
resultOO 
=OO 
awaitOO "
	_mediatorOO# ,
.OO, -
SendOO- 1
(OO1 2
queryOO2 7
)OO7 8
;OO8 9
returnPP 
OkPP 
(PP 
resultPP  
)PP  !
;PP! "
}QQ 
catchRR 
(RR 
	ExceptionRR 
exRR 
)RR  
{SS 
returnTT 

StatusCodeTT !
(TT! "
StatusCodesTT" -
.TT- .(
Status500InternalServerErrorTT. J
,TTJ K
stringTTL R
.TTR S
FormatTTS Y
(TTY Z
ErrorMessagesTTZ g
.TTg h
GeneralServerErrorTTh z
,TTz {
exTT| ~
.TT~ 
Message	TT �
)
TT� �
)
TT� �
;
TT� �
}UU 
}VV 	
[`` 	
HttpPost``	 
]`` 
[aa 	 
ProducesResponseTypeaa	 
(aa 
typeofaa $
(aa$ %
RoleDtoaa% ,
)aa, -
,aa- .
StatusCodesaa/ :
.aa: ;
Status201Createdaa; K
)aaK L
]aaL M
[bb 	 
ProducesResponseTypebb	 
(bb 
StatusCodesbb )
.bb) *
Status400BadRequestbb* =
)bb= >
]bb> ?
[cc 	 
ProducesResponseTypecc	 
(cc 
StatusCodescc )
.cc) *
Status409Conflictcc* ;
)cc; <
]cc< =
publicdd 
asyncdd 
Taskdd 
<dd 
ActionResultdd &
<dd& '
RoleDtodd' .
>dd. /
>dd/ 0

CreateRoledd1 ;
(dd; <
[dd< =
FromBodydd= E
]ddE F
CreateRoleCommandddG X
commandddY `
)dd` a
{ee 	
varff 
resultff 
=ff 
awaitff 
	_mediatorff (
.ff( )
Sendff) -
(ff- .
commandff. 5
)ff5 6
;ff6 7
returngg 
CreatedAtActiongg "
(gg" #
nameofgg# )
(gg) *
GetRolegg* 1
)gg1 2
,gg2 3
newgg4 7
{gg8 9
idgg: <
=gg= >
resultgg? E
.ggE F
IdggF H
}ggI J
,ggJ K
resultggL R
)ggR S
;ggS T
}hh 	
[tt 	
HttpPuttt	 
(tt 
$strtt 
)tt 
]tt 
[uu 	 
ProducesResponseTypeuu	 
(uu 
StatusCodesuu )
.uu) *
Status204NoContentuu* <
)uu< =
]uu= >
[vv 	 
ProducesResponseTypevv	 
(vv 
StatusCodesvv )
.vv) *
Status400BadRequestvv* =
)vv= >
]vv> ?
[ww 	 
ProducesResponseTypeww	 
(ww 
StatusCodesww )
.ww) *
Status404NotFoundww* ;
)ww; <
]ww< =
[xx 	 
ProducesResponseTypexx	 
(xx 
StatusCodesxx )
.xx) *
Status409Conflictxx* ;
)xx; <
]xx< =
publicyy 
asyncyy 
Taskyy 
<yy 
IActionResultyy '
>yy' (

UpdateRoleyy) 3
(yy3 4
intyy4 7
idyy8 :
,yy: ;
[yy< =
FromBodyyy= E
]yyE F
UpdateRoleCommandyyG X
commandBodyyyY d
)yyd e
{zz 	
if{{ 
({{ 
id{{ 
!={{ 
commandBody{{ !
.{{! "
Id{{" $
){{$ %
{|| 
return~~ 

BadRequest~~ !
(~~! "
$str~~" M
)~~M N
;~~N O
} 
await
�� 
	_mediator
�� 
.
�� 
Send
��  
(
��  !
commandBody
��! ,
)
��, -
;
��- .
return
�� 
	NoContent
�� 
(
�� 
)
�� 
;
�� 
}
�� 	
[
�� 	

HttpDelete
��	 
(
�� 
$str
�� 
)
�� 
]
�� 
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) * 
Status204NoContent
��* <
)
��< =
]
��= >
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *
Status404NotFound
��* ;
)
��; <
]
��< =
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *!
Status400BadRequest
��* =
)
��= >
]
��> ?
public
�� 
async
�� 
Task
�� 
<
�� 
IActionResult
�� '
>
��' (

DeleteRole
��) 3
(
��3 4
int
��4 7
id
��8 :
)
��: ;
{
�� 	
var
�� 
command
�� 
=
�� 
new
�� 
DeleteRoleCommand
�� /
(
��/ 0
id
��0 2
)
��2 3
;
��3 4
await
�� 
	_mediator
�� 
.
�� 
Send
��  
(
��  !
command
��! (
)
��( )
;
��) *
return
�� 
	NoContent
�� 
(
�� 
)
�� 
;
�� 
}
�� 	
}
�� 
}�� _
]C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\ResetPasswordController.csW
UC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\ResetController.cs�7
XC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\ProductsController.cs
	namespace 	
Stock
 
. 
API 
. 
Controllers 
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public 

class 
ProductsController #
:$ %
ControllerBase& 4
{ 
private 
readonly 
	IMediator "
	_mediator# ,
;, -
public 
ProductsController !
(! "
	IMediator" +
mediator, 4
)4 5
{ 	
	_mediator 
= 
mediator  
;  !
} 	
[ 	
HttpGet	 
] 
[ 	 
ProducesResponseType	 
( 
typeof $
($ %
IEnumerable% 0
<0 1

ProductDto1 ;
>; <
)< =
,= >
StatusCodes? J
.J K
Status200OKK V
)V W
]W X
public   
async   
Task   
<   
ActionResult   &
<  & '
IEnumerable  ' 2
<  2 3

ProductDto  3 =
>  = >
>  > ?
>  ? @
GetAllProducts  A O
(  O P
)  P Q
{!! 	
var"" 
query"" 
="" 
new"" 
GetAllProductsQuery"" /
(""/ 0
)""0 1
;""1 2
var## 
products## 
=## 
await##  
	_mediator##! *
.##* +
Send##+ /
(##/ 0
query##0 5
)##5 6
;##6 7
return$$ 
Ok$$ 
($$ 
products$$ 
)$$ 
;$$  
}%% 	
[(( 	
HttpGet((	 
((( 
$str(( 
)(( 
](( 
[)) 	 
ProducesResponseType))	 
()) 
typeof)) $
())$ %

ProductDto))% /
)))/ 0
,))0 1
StatusCodes))2 =
.))= >
Status200OK))> I
)))I J
]))J K
[** 	 
ProducesResponseType**	 
(** 
StatusCodes** )
.**) *
Status404NotFound*** ;
)**; <
]**< =
public++ 
async++ 
Task++ 
<++ 
ActionResult++ &
<++& '

ProductDto++' 1
>++1 2
>++2 3
GetProductById++4 B
(++B C
int++C F
id++G I
)++I J
{,, 	
var-- 
query-- 
=-- 
new-- 
GetProductByIdQuery-- /
(--/ 0
id--0 2
)--2 3
;--3 4
var.. 
product.. 
=.. 
await.. 
	_mediator..  )
...) *
Send..* .
(... /
query../ 4
)..4 5
;..5 6
return11 
Ok11 
(11 
product11 
)11 
;11 
}22 	
[55 	
HttpPost55	 
]55 
[66 	 
ProducesResponseType66	 
(66 
typeof66 $
(66$ %

ProductDto66% /
)66/ 0
,660 1
StatusCodes662 =
.66= >
Status201Created66> N
)66N O
]66O P
[77 	 
ProducesResponseType77	 
(77 
StatusCodes77 )
.77) *
Status400BadRequest77* =
)77= >
]77> ?
public88 
async88 
Task88 
<88 
ActionResult88 &
<88& '

ProductDto88' 1
>881 2
>882 3
CreateProduct884 A
(88A B
[88B C
FromBody88C K
]88K L 
CreateProductCommand88M a
command88b i
)88i j
{99 	
var:: 
product:: 
=:: 
await:: 
	_mediator::  )
.::) *
Send::* .
(::. /
command::/ 6
)::6 7
;::7 8
return;; 
CreatedAtAction;; "
(;;" #
nameof;;# )
(;;) *
GetProductById;;* 8
);;8 9
,;;9 :
new;;; >
{;;? @
id;;A C
=;;D E
product;;F M
.;;M N
Id;;N P
};;Q R
,;;R S
product;;T [
);;[ \
;;;\ ]
}<< 	
[?? 	
HttpPut??	 
(?? 
$str?? 
)?? 
]?? 
[@@ 	 
ProducesResponseType@@	 
(@@ 
StatusCodes@@ )
.@@) *
Status204NoContent@@* <
)@@< =
]@@= >
[AA 	 
ProducesResponseTypeAA	 
(AA 
StatusCodesAA )
.AA) *
Status400BadRequestAA* =
)AA= >
]AA> ?
[BB 	 
ProducesResponseTypeBB	 
(BB 
StatusCodesBB )
.BB) *
Status404NotFoundBB* ;
)BB; <
]BB< =
publicCC 
asyncCC 
TaskCC 
<CC 
IActionResultCC '
>CC' (
UpdateProductCC) 6
(CC6 7
intCC7 :
idCC; =
,CC= >
[CC? @
FromBodyCC@ H
]CCH I 
UpdateProductCommandCCJ ^
commandCC_ f
)CCf g
{DD 	
ifFF 
(FF 
idFF 
!=FF 
commandFF 
.FF 
IdFF  
)FF  !
{GG 
returnHH 

BadRequestHH !
(HH! "
$strHH" Y
)HHY Z
;HHZ [
}II 
awaitJJ 
	_mediatorJJ 
.JJ 
SendJJ  
(JJ  !
commandJJ! (
)JJ( )
;JJ) *
returnKK 
	NoContentKK 
(KK 
)KK 
;KK 
}LL 	
[OO 	

HttpDeleteOO	 
(OO 
$strOO 
)OO 
]OO  
[PP 	 
ProducesResponseTypePP	 
(PP 
StatusCodesPP )
.PP) *
Status204NoContentPP* <
)PP< =
]PP= >
[QQ 	 
ProducesResponseTypeQQ	 
(QQ 
StatusCodesQQ )
.QQ) *
Status404NotFoundQQ* ;
)QQ; <
]QQ< =
publicRR 
asyncRR 
TaskRR 
<RR 
IActionResultRR '
>RR' (
DeleteProductRR) 6
(RR6 7
intRR7 :
idRR; =
)RR= >
{SS 	
varTT 
commandTT 
=TT 
newTT  
DeleteProductCommandTT 2
(TT2 3
idTT3 5
)TT5 6
;TT6 7
awaitUU 
	_mediatorUU 
.UU 
SendUU  
(UU  !
commandUU! (
)UU( )
;UU) *
returnVV 
	NoContentVV 
(VV 
)VV 
;VV 
}WW 	
}XX 
}YY ��
[C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\PermissionsController.cs
	namespace 	
Stock
 
. 
API 
. 
Controllers 
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
ApiConstants 
. 
ApiBaseRoute $
+% &
$str' 6
)6 7
]7 8
[ 
	Authorize 
] 
public 

class !
PermissionsController &
:' (
ControllerBase) 7
{ 
private 
readonly 
IPermissionService +
_permissionService, >
;> ?
private 
readonly  
ApplicationDbContext -
_context. 6
;6 7
private   
readonly   
ILogger    
<    !!
PermissionsController  ! 6
>  6 7
_logger  8 ?
;  ? @
private!! 
readonly!! 
	IMediator!! "
	_mediator!!# ,
;!!, -
public++ !
PermissionsController++ $
(++$ %
IPermissionService,, 
permissionService,, 0
,,,0 1 
ApplicationDbContext--  
context--! (
,--( )
ILogger.. 
<.. !
PermissionsController.. )
>..) *
logger..+ 1
,..1 2
	IMediator// 
mediator// 
)// 
{00 	
_permissionService11 
=11  
permissionService11! 2
;112 3
_context22 
=22 
context22 
;22 
_logger33 
=33 
logger33 
;33 
	_mediator44 
=44 
mediator44  
;44  !
}55 	
[<< 	
HttpGet<<	 
]<< 
[== 	 
ProducesResponseType==	 
(== 
typeof== $
(==$ %
IEnumerable==% 0
<==0 1
PermissionDto==1 >
>==> ?
)==? @
,==@ A
StatusCodes==B M
.==M N
Status200OK==N Y
)==Y Z
]==Z [
public>> 
async>> 
Task>> 
<>> 
ActionResult>> &
<>>& '
IEnumerable>>' 2
<>>2 3
PermissionDto>>3 @
>>>@ A
>>>A B
>>>B C
GetAllPermissions>>D U
(>>U V
)>>V W
{?? 	
var@@ 
query@@ 
=@@ 
new@@ "
GetAllPermissionsQuery@@ 2
(@@2 3
)@@3 4
;@@4 5
varAA 
resultAA 
=AA 
awaitAA 
	_mediatorAA (
.AA( )
SendAA) -
(AA- .
queryAA. 3
)AA3 4
;AA4 5
returnBB 
OkBB 
(BB 
resultBB 
)BB 
;BB 
}CC 	
[KK 	
HttpGetKK	 
(KK 
$strKK 
)KK 
]KK 
[LL 	
	AuthorizeLL	 
(LL 
RolesLL 
=LL 
	RoleNamesLL $
.LL$ %
AdminLL% *
)LL* +
]LL+ ,
[MM 	 
ProducesResponseTypeMM	 
(MM 
typeofMM $
(MM$ %
ListMM% )
<MM) *
PermissionGroupDtoMM* <
>MM< =
)MM= >
,MM> ?
StatusCodesMM@ K
.MMK L
Status200OKMML W
)MMW X
]MMX Y
[NN 	 
ProducesResponseTypeNN	 
(NN 
StatusCodesNN )
.NN) *!
Status401UnauthorizedNN* ?
)NN? @
]NN@ A
publicOO 
asyncOO 
TaskOO 
<OO 
ActionResultOO &
<OO& '
ListOO' +
<OO+ ,
PermissionGroupDtoOO, >
>OO> ?
>OO? @
>OO@ A
GetByGroupsOOB M
(OOM N
)OON O
{PP 	
varQQ 
permissionGroupsQQ  
=QQ! "
awaitQQ# (
_permissionServiceQQ) ;
.QQ; <'
GetPermissionsByGroupsAsyncQQ< W
(QQW X
)QQX Y
;QQY Z
returnRR 
OkRR 
(RR 
permissionGroupsRR &
)RR& '
;RR' (
}SS 	
[\\ 	
HttpGet\\	 
(\\ 
$str\\  
)\\  !
]\\! "
[]] 	
	Authorize]]	 
(]] 
Roles]] 
=]] 
	RoleNames]] $
.]]$ %
Admin]]% *
)]]* +
]]]+ ,
[^^ 	 
ProducesResponseType^^	 
(^^ 
typeof^^ $
(^^$ %
List^^% )
<^^) *
PermissionDto^^* 7
>^^7 8
)^^8 9
,^^9 :
StatusCodes^^; F
.^^F G
Status200OK^^G R
)^^R S
]^^S T
[__ 	 
ProducesResponseType__	 
(__ 
StatusCodes__ )
.__) *!
Status401Unauthorized__* ?
)__? @
]__@ A
public`` 
async`` 
Task`` 
<`` 
ActionResult`` &
<``& '
List``' +
<``+ ,
PermissionDto``, 9
>``9 :
>``: ;
>``; <
GetByRoleId``= H
(``H I
int``I L
roleId``M S
)``S T
{aa 	
varbb 
permissionsbb 
=bb 
awaitbb #
_permissionServicebb$ 6
.bb6 7'
GetPermissionsByRoleIdAsyncbb7 R
(bbR S
roleIdbbS Y
)bbY Z
;bbZ [
returncc 
Okcc 
(cc 
permissionscc !
)cc! "
;cc" #
}dd 	
[oo 	
HttpGetoo	 
(oo 
$stroo  
)oo  !
]oo! "
[pp 	
RequirePermissionpp	 
(pp 
PermissionNamespp *
.pp* +
PermissionsViewpp+ :
)pp: ;
]pp; <
[qq 	 
ProducesResponseTypeqq	 
(qq 
typeofqq $
(qq$ %
Listqq% )
<qq) *
PermissionDtoqq* 7
>qq7 8
)qq8 9
,qq9 :
StatusCodesqq; F
.qqF G
Status200OKqqG R
)qqR S
]qqS T
[rr 	 
ProducesResponseTyperr	 
(rr 
StatusCodesrr )
.rr) *!
Status401Unauthorizedrr* ?
)rr? @
]rr@ A
[ss 	 
ProducesResponseTypess	 
(ss 
StatusCodesss )
.ss) *
Status403Forbiddenss* <
)ss< =
]ss= >
publictt 
asynctt 
Tasktt 
<tt 
ActionResulttt &
<tt& '
Listtt' +
<tt+ ,
PermissionDtott, 9
>tt9 :
>tt: ;
>tt; <
GetByUserIdtt= H
(ttH I
intttI L
userIdttM S
)ttS T
{uu 	
varvv 
permissionsvv 
=vv 
awaitvv #
_permissionServicevv$ 6
.vv6 7'
GetPermissionsByUserIdAsyncvv7 R
(vvR S
userIdvvS Y
)vvY Z
;vvZ [
returnww 
Okww 
(ww 
permissionsww !
)ww! "
;ww" #
}xx 	
[
�� 	
HttpPost
��	 
(
�� 
$str
�� (
)
��( )
]
��) *
[
�� 	
RequirePermission
��	 
(
�� 
PermissionNames
�� *
.
��* +
	RolesEdit
��+ 4
)
��4 5
]
��5 6
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
bool
��% )
)
��) *
,
��* +
StatusCodes
��, 7
.
��7 8
Status200OK
��8 C
)
��C D
]
��D E
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *#
Status401Unauthorized
��* ?
)
��? @
]
��@ A
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) * 
Status403Forbidden
��* <
)
��< =
]
��= >
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *
Status404NotFound
��* ;
)
��; <
]
��< =
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) **
Status500InternalServerError
��* F
)
��F G
]
��G H
public
�� 
async
�� 
Task
�� 
<
�� 
ActionResult
�� &
<
��& '
bool
��' +
>
��+ ,
>
��, -%
AssignPermissionsToRole
��. E
(
��E F
int
��F I
roleId
��J P
,
��P Q
[
��R S
FromBody
��S [
]
��[ \&
AssignPermissionsRequest
��] u
request
��v }
)
��} ~
{
�� 	
var
�� 
result
�� 
=
�� 
await
��  
_permissionService
�� 1
.
��1 2*
AssignPermissionsToRoleAsync
��2 N
(
��N O
roleId
��O U
,
��U V
request
��W ^
.
��^ _
PermissionIds
��_ l
)
��l m
;
��m n
return
�� 
Ok
�� 
(
�� 
result
�� 
)
�� 
;
�� 
}
�� 	
[
�� 	
HttpPost
��	 
(
�� 
$str
�� (
)
��( )
]
��) *
[
�� 	
RequirePermission
��	 
(
�� 
PermissionNames
�� *
.
��* +$
UsersPermissionsAssign
��+ A
)
��A B
]
��B C
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
bool
��% )
)
��) *
,
��* +
StatusCodes
��, 7
.
��7 8
Status200OK
��8 C
)
��C D
]
��D E
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *#
Status401Unauthorized
��* ?
)
��? @
]
��@ A
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) * 
Status403Forbidden
��* <
)
��< =
]
��= >
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *
Status404NotFound
��* ;
)
��; <
]
��< =
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :*
Status500InternalServerError
��: V
)
��V W
]
��W X
public
�� 
async
�� 
Task
�� 
<
�� 
ActionResult
�� &
<
��& '
bool
��' +
>
��+ ,
>
��, -%
AssignPermissionsToUser
��. E
(
��E F
int
��F I
userId
��J P
,
��P Q
[
��R S
FromBody
��S [
]
��[ \&
AssignPermissionsRequest
��] u
request
��v }
)
��} ~
{
�� 	
try
�� 
{
�� 
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A%
UserPermissionAssigning
��A X
,
��X Y
userId
��Z `
,
��` a
request
��b i
.
��i j
PermissionIds
��j w
.
��w x
Count
��x }
)
��} ~
)
��~ 
;�� �
bool
�� 
result
�� 
=
�� 
true
�� "
;
��" #
foreach
�� 
(
�� 
var
�� 
permissionId
�� )
in
��* ,
request
��- 4
.
��4 5
PermissionIds
��5 B
)
��B C
{
�� 
var
�� 
assignResult
�� $
=
��% &
await
��' , 
_permissionService
��- ?
.
��? @)
AssignPermissionToUserAsync
��@ [
(
��[ \
userId
��\ b
,
��b c
permissionId
��d p
,
��p q
true
��r v
)
��v w
;
��w x
if
�� 
(
�� 
!
�� 
assignResult
�� %
)
��% &
{
�� 
result
�� 
=
��  
false
��! &
;
��& '
_logger
�� 
.
��  

LogWarning
��  *
(
��* +
string
��+ 1
.
��1 2
Format
��2 8
(
��8 9
LogMessages
��9 D
.
��D E$
PermissionAssignFailed
��E [
,
��[ \
userId
��] c
,
��c d
permissionId
��e q
)
��q r
)
��r s
;
��s t
}
�� 
}
�� 
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A%
UserPermissionsAssigned
��A X
,
��X Y
userId
��Z `
)
��` a
)
��a b
;
��b c
return
�� 
Ok
�� 
(
�� 
result
��  
)
��  !
;
��! "
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
��# $
string
��% +
.
��+ ,
Format
��, 2
(
��2 3
LogMessages
��3 >
.
��> ?'
UserPermissionAssignError
��? X
,
��X Y
userId
��Z `
)
��` a
)
��a b
;
��b c
return
�� 

StatusCode
�� !
(
��! "
StatusCodes
��" -
.
��- .*
Status500InternalServerError
��. J
,
��J K
new
��L O
{
��P Q
error
��R W
=
��X Y
string
��Z `
.
��` a
Format
��a g
(
��g h
ErrorMessages
��h u
.
��u v(
UserPermissionAssignError��v �
,��� �
ex��� �
.��� �
Message��� �
)��� �
}��� �
)��� �
;��� �
}
�� 
}
�� 	
[
�� 	
HttpPost
��	 
(
�� 
$str
�� 7
)
��7 8
]
��8 9
[
�� 	
RequirePermission
��	 
(
�� 
PermissionNames
�� *
.
��* +$
UsersPermissionsAssign
��+ A
)
��A B
]
��B C
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
bool
��% )
)
��) *
,
��* +
StatusCodes
��, 7
.
��7 8
Status200OK
��8 C
)
��C D
]
��D E
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *#
Status401Unauthorized
��* ?
)
��? @
]
��@ A
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) * 
Status403Forbidden
��* <
)
��< =
]
��= >
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *
Status404NotFound
��* ;
)
��; <
]
��< =
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) **
Status500InternalServerError
��* F
)
��F G
]
��G H
public
�� 
async
�� 
Task
�� 
<
�� 
ActionResult
�� &
<
��& '
bool
��' +
>
��+ ,
>
��, -$
AssignPermissionToUser
��. D
(
��D E
int
��E H
userId
��I O
,
��O P
int
��Q T
permissionId
��U a
,
��a b
[
��c d
	FromQuery
��d m
]
��m n
bool
��o s
	isGranted
��t }
=
��~ 
true��� �
)��� �
{
�� 	
var
�� 
result
�� 
=
�� 
await
��  
_permissionService
�� 1
.
��1 2)
AssignPermissionToUserAsync
��2 M
(
��M N
userId
��N T
,
��T U
permissionId
��V b
,
��b c
	isGranted
��d m
)
��m n
;
��n o
return
�� 
Ok
�� 
(
�� 
result
�� 
)
�� 
;
�� 
}
�� 	
[
�� 	

HttpDelete
��	 
(
�� 
$str
�� =
)
��= >
]
��> ?
[
�� 	
RequirePermission
��	 
(
�� 
PermissionNames
�� *
.
��* +$
UsersPermissionsRemove
��+ A
)
��A B
]
��B C
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
bool
��% )
)
��) *
,
��* +
StatusCodes
��, 7
.
��7 8
Status200OK
��8 C
)
��C D
]
��D E
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *#
Status401Unauthorized
��* ?
)
��? @
]
��@ A
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) * 
Status403Forbidden
��* <
)
��< =
]
��= >
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *
Status404NotFound
��* ;
)
��; <
]
��< =
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) **
Status500InternalServerError
��* F
)
��F G
]
��G H
public
�� 
async
�� 
Task
�� 
<
�� 
ActionResult
�� &
<
��& '
bool
��' +
>
��+ ,
>
��, -"
RemoveUserPermission
��. B
(
��B C
int
��C F
userId
��G M
,
��M N
int
��O R
permissionId
��S _
)
��_ `
{
�� 	
var
�� 
result
�� 
=
�� 
await
��  
_permissionService
�� 1
.
��1 2'
RemoveUserPermissionAsync
��2 K
(
��K L
userId
��L R
,
��R S
permissionId
��T `
)
��` a
;
��a b
return
�� 
Ok
�� 
(
�� 
result
�� 
)
�� 
;
�� 
}
�� 	
[
�� 	
HttpPost
��	 
(
�� 
$str
�� '
)
��' (
]
��( )
[
�� 	
RequirePermission
��	 
(
�� 
PermissionNames
�� *
.
��* +#
UsersPermissionsReset
��+ @
)
��@ A
]
��A B
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
bool
��% )
)
��) *
,
��* +
StatusCodes
��, 7
.
��7 8
Status200OK
��8 C
)
��C D
]
��D E
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *#
Status401Unauthorized
��* ?
)
��? @
]
��@ A
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) * 
Status403Forbidden
��* <
)
��< =
]
��= >
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *
Status404NotFound
��* ;
)
��; <
]
��< =
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) **
Status500InternalServerError
��* F
)
��F G
]
��G H
public
�� 
async
�� 
Task
�� 
<
�� 
ActionResult
�� &
<
��& '
bool
��' +
>
��+ ,
>
��, -(
ResetUserToRolePermissions
��. H
(
��H I
int
��I L
userId
��M S
)
��S T
{
�� 	
var
�� 
result
�� 
=
�� 
await
��  
_permissionService
�� 1
.
��1 2-
ResetUserToRolePermissionsAsync
��2 Q
(
��Q R
userId
��R X
)
��X Y
;
��Y Z
return
�� 
Ok
�� 
(
�� 
result
�� 
)
�� 
;
�� 
}
�� 	
[
�� 	
HttpGet
��	 
(
�� 
$str
�� 7
)
��7 8
]
��8 9
[
�� 	
RequirePermission
��	 
(
�� 
PermissionNames
�� *
.
��* +
PermissionsCheck
��+ ;
)
��; <
]
��< =
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
bool
��% )
)
��) *
,
��* +
StatusCodes
��, 7
.
��7 8
Status200OK
��8 C
)
��C D
]
��D E
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *#
Status401Unauthorized
��* ?
)
��? @
]
��@ A
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) * 
Status403Forbidden
��* <
)
��< =
]
��= >
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *
Status404NotFound
��* ;
)
��; <
]
��< =
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) **
Status500InternalServerError
��* F
)
��F G
]
��G H
public
�� 
async
�� 
Task
�� 
<
�� 
ActionResult
�� &
<
��& '
bool
��' +
>
��+ ,
>
��, -
UserHasPermission
��. ?
(
��? @
int
��@ C
userId
��D J
,
��J K
string
��L R
permissionName
��S a
)
��a b
{
�� 	
var
�� 
result
�� 
=
�� 
await
��  
_permissionService
�� 1
.
��1 2$
UserHasPermissionAsync
��2 H
(
��H I
userId
��I O
,
��O P
permissionName
��Q _
)
��_ `
;
��` a
return
�� 
Ok
�� 
(
�� 
result
�� 
)
�� 
;
�� 
}
�� 	
[
�� 	
HttpGet
��	 
(
�� 
$str
�� *
)
��* +
]
��+ ,
[
�� 	
	Authorize
��	 
(
�� 
Roles
�� 
=
�� 
	RoleNames
�� $
.
��$ %
Admin
��% *
)
��* +
]
��+ ,
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :
Status200OK
��: E
)
��E F
]
��F G
[
�� 	"
ProducesResponseType
��	 
(
�� 
StatusCodes
�� )
.
��) *#
Status401Unauthorized
��* ?
)
��? @
]
��@ A
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
string
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :*
Status500InternalServerError
��: V
)
��V W
]
��W X
public
�� 
async
�� 
Task
�� 
<
�� 
ActionResult
�� &
>
��& '+
AddMissingPermissionsManually
��( E
(
��E F
)
��F G
{
�� 	
try
�� 
{
�� 
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
LogMessages
��' 2
.
��2 3&
AddingMissingPermissions
��3 K
)
��K L
;
��L M
await
�� +
AddPermissionIfNotExistsAsync
�� 3
(
��3 4
PermissionNames
��4 C
.
��C D
PagesRevirView
��D R
,
��R S
$str
��T q
,
��q r
$str��s �
,��� �
$str��� �
,��� �
$str��� �
,��� �
$str��� �
)��� �
;��� �
await
�� +
AddPermissionIfNotExistsAsync
�� 3
(
��3 4
PermissionNames
��4 C
.
��C D!
PagesBilgiIslemView
��D W
,
��W X
$str
��Y |
,
��| }
$str��~ �
,��� �
$str��� �
,��� �
$str��� �
,��� �
$str��� �
)��� �
;��� �
await
�� 
_context
�� 
.
�� 
SaveChangesAsync
�� /
(
��/ 0
)
��0 1
;
��1 2
return
�� 
Ok
�� 
(
�� 
new
�� 
{
�� 
message
��  '
=
��( )
ApiConstants
��* 6
.
��6 7
SuccessMessage
��7 E
}
��F G
)
��G H
;
��H I
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
ErrorAddingMissingPermissions
��1 N
)
��N O
;
��O P
return
�� 

StatusCode
�� !
(
��! "
StatusCodes
��" -
.
��- .*
Status500InternalServerError
��. J
,
��J K
string
��L R
.
��R S
Format
��S Y
(
��Y Z
ErrorMessages
��Z g
.
��g h%
MissingPermissionsError
��h 
,�� �
ex��� �
.��� �
Message��� �
)��� �
)��� �
;��� �
}
�� 
}
�� 	
private
�� 
async
�� 
Task
�� +
AddPermissionIfNotExistsAsync
�� 8
(
��8 9
string
��9 ?
name
��@ D
,
��D E
string
��F L
description
��M X
,
��X Y
string
��Z `
group
��a f
,
��f g
string
��h n
resourceType
��o {
,
��{ |
string��} �
resourceName��� �
,��� �
string��� �
action��� �
)��� �
{
�� 	
var
�� 
permissionExists
��  
=
��! "
await
��# (
_context
��) 1
.
��1 2
Permissions
��2 =
.
��= >
AnyAsync
��> F
(
��F G
p
��G H
=>
��I K
p
��L M
.
��M N
Name
��N R
==
��S U
name
��V Z
)
��Z [
;
��[ \
if
�� 
(
�� 
!
�� 
permissionExists
�� !
)
��! "
{
�� 
_context
�� 
.
�� 
Permissions
�� $
.
��$ %
Add
��% (
(
��( )
new
��) ,

Permission
��- 7
{
�� 
Name
�� 
=
�� 
name
�� 
,
��  
Description
�� 
=
��  !
description
��" -
,
��- .
Group
�� 
=
�� 
group
�� !
,
��! "
ResourceType
��  
=
��! "
resourceType
��# /
,
��/ 0
ResourceName
��  
=
��! "
resourceName
��# /
,
��/ 0
Action
�� 
=
�� 
action
�� #
,
��# $
	CreatedAt
�� 
=
�� 
DateTime
��  (
.
��( )
UtcNow
��) /
}
�� 
)
�� 
;
�� 
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A
PermissionAdded
��A P
,
��P Q
name
��R V
)
��V W
)
��W X
;
��X Y
}
�� 
else
�� 
{
�� 
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
string
��' -
.
��- .
Format
��. 4
(
��4 5
LogMessages
��5 @
.
��@ A%
PermissionAlreadyExists
��A X
,
��X Y
name
��Z ^
)
��^ _
)
��_ `
;
��` a
}
�� 
}
�� 	
}
�� 
public
�� 

class
�� &
AssignPermissionsRequest
�� )
{
�� 
public
�� 
List
�� 
<
�� 
int
�� 
>
�� 
PermissionIds
�� &
{
��' (
get
��) ,
;
��, -
set
��. 1
;
��1 2
}
��3 4
=
��5 6
new
��7 :
List
��; ?
<
��? @
int
��@ C
>
��C D
(
��D E
)
��E F
;
��F G
}
�� 
}�� [
YC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\DirectFixController.csY
WC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\DbCheckController.cs��
TC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\AuthController.cs
	namespace 	
Stock
 
. 
API 
. 
Controllers 
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
ApiConstants 
. 
ApiBaseRoute $
+% &
$str' 6
)6 7
]7 8
public 

class 
AuthController 
:  !
ControllerBase" 0
{ 
private 
readonly 
IAuthService %
_authService& 2
;2 3
private 
readonly 
ILogger  
<  !
AuthController! /
>/ 0
_logger1 8
;8 9
private 
readonly 
	IMediator "
	_mediator# ,
;, -
public"" 
AuthController"" 
("" 
IAuthService"" *
authService""+ 6
,""6 7
ILogger""8 ?
<""? @
AuthController""@ N
>""N O
logger""P V
,""V W
	IMediator""X a
mediator""b j
)""j k
{## 	
_authService$$ 
=$$ 
authService$$ &
;$$& '
_logger%% 
=%% 
logger%% 
;%% 
	_mediator&& 
=&& 
mediator&&  
;&&  !
}'' 	
[33 	
HttpPost33	 
(33 
$str33 
)33 
]33 
[44 	 
ProducesResponseType44	 
(44 
typeof44 $
(44$ %
AuthResponseDto44% 4
)444 5
,445 6
StatusCodes447 B
.44B C
Status200OK44C N
)44N O
]44O P
[55 	 
ProducesResponseType55	 
(55 
typeof55 $
(55$ %
AuthResponseDto55% 4
)554 5
,555 6
StatusCodes557 B
.55B C!
Status401Unauthorized55C X
)55X Y
]55Y Z
public66 
async66 
Task66 
<66 
IActionResult66 '
>66' (
Login66) .
(66. /
[66/ 0
FromBody660 8
]668 9
LoginDto66: B
loginDto66C K
)66K L
{77 	
var88 
result88 
=88 
await88 
_authService88 +
.88+ ,

LoginAsync88, 6
(886 7
loginDto887 ?
)88? @
;88@ A
if:: 
(:: 
!:: 
result:: 
.:: 
Success:: 
)::  
return;; 
Unauthorized;; #
(;;# $
result;;$ *
);;* +
;;;+ ,
return== 
Ok== 
(== 
result== 
)== 
;== 
}>> 	
[JJ 	
HttpPostJJ	 
(JJ 
$strJJ 
)JJ 
]JJ 
[KK 	 
ProducesResponseTypeKK	 
(KK 
typeofKK $
(KK$ %
AuthResponseDtoKK% 4
)KK4 5
,KK5 6
StatusCodesKK7 B
.KKB C
Status200OKKKC N
)KKN O
]KKO P
[LL 	 
ProducesResponseTypeLL	 
(LL 
typeofLL $
(LL$ %
AuthResponseDtoLL% 4
)LL4 5
,LL5 6
StatusCodesLL7 B
.LLB C
Status400BadRequestLLC V
)LLV W
]LLW X
publicMM 
asyncMM 
TaskMM 
<MM 
IActionResultMM '
>MM' (
RegisterMM) 1
(MM1 2
[MM2 3
FromBodyMM3 ;
]MM; <
RegisterDtoMM= H
registerDtoMMI T
)MMT U
{NN 	
varOO 
resultOO 
=OO 
awaitOO 
_authServiceOO +
.OO+ ,
RegisterAsyncOO, 9
(OO9 :
registerDtoOO: E
)OOE F
;OOF G
ifQQ 
(QQ 
!QQ 
resultQQ 
.QQ 
SuccessQQ 
)QQ  
returnRR 

BadRequestRR !
(RR! "
resultRR" (
)RR( )
;RR) *
returnTT 
OkTT 
(TT 
resultTT 
)TT 
;TT 
}UU 	
[cc 	
	Authorizecc	 
(cc 
Rolescc 
=cc 
	RoleNamescc $
.cc$ %
Admincc% *
)cc* +
]cc+ ,
[dd 	
HttpPostdd	 
(dd 
$strdd 
)dd  
]dd  !
[ee 	 
ProducesResponseTypeee	 
(ee 
typeofee $
(ee$ %
UserDtoee% ,
)ee, -
,ee- .
StatusCodesee/ :
.ee: ;
Status201Createdee; K
)eeK L
]eeL M
[ff 	 
ProducesResponseTypeff	 
(ff 
typeofff $
(ff$ %
objectff% +
)ff+ ,
,ff, -
StatusCodesff. 9
.ff9 :
Status400BadRequestff: M
)ffM N
]ffN O
[gg 	 
ProducesResponseTypegg	 
(gg 
StatusCodesgg )
.gg) *!
Status401Unauthorizedgg* ?
)gg? @
]gg@ A
[hh 	 
ProducesResponseTypehh	 
(hh 
typeofhh $
(hh$ %
objecthh% +
)hh+ ,
,hh, -
StatusCodeshh. 9
.hh9 :(
Status500InternalServerErrorhh: V
)hhV W
]hhW X
publicii 
asyncii 
Taskii 
<ii 
IActionResultii '
>ii' (

CreateUserii) 3
(ii3 4
[ii4 5
FromBodyii5 =
]ii= >
CreateUserCommandii? P
commandiiQ X
)iiX Y
{jj 	
trykk 
{ll 
_loggermm 
.mm 
LogInformationmm &
(mm& '
LogMessagesmm' 2
.mm2 3
UserCreationRequestmm3 F
,mmF G
$"mmH J
{mmJ K
commandmmK R
.mmR S
AdimmS V
}mmV W
$strmmW X
{mmX Y
commandmmY `
.mm` a
Soyadimma g
}mmg h
"mmh i
,mmi j
commandmmk r
.mmr s
Sicilmms x
)mmx y
;mmy z
ifpp 
(pp 
stringpp 
.pp 
IsNullOrWhiteSpacepp -
(pp- .
commandpp. 5
.pp5 6
Adipp6 9
)pp9 :
)pp: ;
{qq 
_loggerrr 
.rr 

LogWarningrr &
(rr& '
LogMessagesrr' 2
.rr2 3
MissingParameterrr3 C
,rrC D
$strrrE J
)rrJ K
;rrK L
returnss 

BadRequestss %
(ss% &
newss& )
{ss* +
errorss, 1
=ss2 3
ErrorMessagesss4 A
.ssA B
	NameEmptyssB K
,ssK L
fieldssM R
=ssS T
$strssU Z
}ss[ \
)ss\ ]
;ss] ^
}tt 
ifvv 
(vv 
stringvv 
.vv 
IsNullOrWhiteSpacevv -
(vv- .
commandvv. 5
.vv5 6
Soyadivv6 <
)vv< =
)vv= >
{ww 
_loggerxx 
.xx 

LogWarningxx &
(xx& '
LogMessagesxx' 2
.xx2 3
MissingParameterxx3 C
,xxC D
$strxxE M
)xxM N
;xxN O
returnyy 

BadRequestyy %
(yy% &
newyy& )
{yy* +
erroryy, 1
=yy2 3
ErrorMessagesyy4 A
.yyA B
SurnameEmptyyyB N
,yyN O
fieldyyP U
=yyV W
$stryyX `
}yya b
)yyb c
;yyc d
}zz 
if|| 
(|| 
string|| 
.|| 
IsNullOrWhiteSpace|| -
(||- .
command||. 5
.||5 6
Password||6 >
)||> ?
)||? @
{}} 
_logger~~ 
.~~ 

LogWarning~~ &
(~~& '
LogMessages~~' 2
.~~2 3
MissingParameter~~3 C
,~~C D
$str~~E O
)~~O P
;~~P Q
return 

BadRequest %
(% &
new& )
{* +
error, 1
=2 3
ErrorMessages4 A
.A B
PasswordEmptyB O
,O P
fieldQ V
=W X
$strY c
}d e
)e f
;f g
}
�� 
if
�� 
(
�� 
string
�� 
.
��  
IsNullOrWhiteSpace
�� -
(
��- .
command
��. 5
.
��5 6
Sicil
��6 ;
)
��; <
)
��< =
{
�� 
_logger
�� 
.
�� 

LogWarning
�� &
(
��& '
LogMessages
��' 2
.
��2 3
MissingParameter
��3 C
,
��C D
$str
��E L
)
��L M
;
��M N
return
�� 

BadRequest
�� %
(
��% &
new
��& )
{
��* +
error
��, 1
=
��2 3
ErrorMessages
��4 A
.
��A B

SicilEmpty
��B L
,
��L M
field
��N S
=
��T U
$str
��V ]
}
��^ _
)
��_ `
;
��` a
}
�� 
if
�� 
(
�� 
!
�� 
command
�� 
.
�� 
RoleId
�� #
.
��# $
HasValue
��$ ,
||
��- /
command
��0 7
.
��7 8
RoleId
��8 >
<=
��? A
$num
��B C
)
��C D
{
�� 
_logger
�� 
.
�� 

LogWarning
�� &
(
��& '
LogMessages
��' 2
.
��2 3
InvalidRoleId
��3 @
,
��@ A
command
��B I
.
��I J
RoleId
��J P
)
��P Q
;
��Q R
return
�� 

BadRequest
�� %
(
��% &
new
��& )
{
��* +
error
��, 1
=
��2 3
ErrorMessages
��4 A
.
��A B
ValidRoleRequired
��B S
,
��S T
field
��U Z
=
��[ \
$str
��] e
}
��f g
)
��g h
;
��h i
}
�� 
var
�� 
result
�� 
=
�� 
await
�� "
	_mediator
��# ,
.
��, -
Send
��- 1
(
��1 2
command
��2 9
)
��9 :
;
��: ;
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
LogMessages
��' 2
.
��2 3%
UserCreatedSuccessfully
��3 J
,
��J K
$"
��L N
{
��N O
result
��O U
.
��U V
Adi
��V Y
}
��Y Z
$str
��Z [
{
��[ \
result
��\ b
.
��b c
Soyadi
��c i
}
��i j
"
��j k
,
��k l
result
��m s
.
��s t
Id
��t v
)
��v w
;
��w x
return
�� 
CreatedAtAction
�� &
(
��& '
nameof
��' -
(
��- .

CreateUser
��. 8
)
��8 9
,
��9 :
new
��; >
{
��? @
id
��A C
=
��D E
result
��F L
.
��L M
Id
��M O
}
��P Q
,
��Q R
result
��S Y
)
��Y Z
;
��Z [
}
�� 
catch
�� 
(
�� '
InvalidOperationException
�� ,
ex
��- /
)
��/ 0
{
�� 
string
�� 
errorMessage
�� #
=
��$ %
ex
��& (
.
��( )
Message
��) 0
;
��0 1
_logger
�� 
.
�� 

LogWarning
�� "
(
��" #
LogMessages
��# .
.
��. //
!ValidationErrorDuringUserCreation
��/ P
,
��P Q
errorMessage
��R ^
)
��^ _
;
��_ `
if
�� 
(
�� 
errorMessage
��  
.
��  !
Contains
��! )
(
��) *
ErrorMessages
��* 7
.
��7 8&
SicilAlreadyInUsePartial
��8 P
)
��P Q
)
��Q R
{
�� 
return
�� 

BadRequest
�� %
(
��% &
new
��& )
{
��* +
error
�� 
=
�� 
string
��  &
.
��& '
Format
��' -
(
��- .
ErrorMessages
��. ;
.
��; <
SicilConflict
��< I
,
��I J
errorMessage
��K W
)
��W X
,
��X Y
code
�� 
=
�� 
$str
�� /
,
��/ 0
field
�� 
=
�� 
$str
��  '
}
�� 
)
�� 
;
�� 
}
�� 
return
�� 

BadRequest
�� !
(
��! "
new
��" %
{
��& '
error
��( -
=
��. /
errorMessage
��0 <
}
��= >
)
��> ?
;
��? @
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
��0 1%
ErrorDuringUserCreation
��1 H
,
��H I
ex
��J L
.
��L M
Message
��M T
)
��T U
;
��U V
if
�� 
(
�� 
ex
�� 
.
�� 
InnerException
�� %
!=
��& (
null
��) -
)
��- .
{
�� 
_logger
�� 
.
�� 
LogError
�� $
(
��$ %
LogMessages
��% 0
.
��0 1

InnerError
��1 ;
,
��; <
ex
��= ?
.
��? @
InnerException
��@ N
.
��N O
Message
��O V
)
��V W
;
��W X
}
�� 
return
�� 

StatusCode
�� !
(
��! "
$num
��" %
,
��% &
new
��' *
{
��+ ,
error
�� 
=
�� 
ErrorMessages
�� )
.
��) *
UserCreationError
��* ;
,
��; <
details
�� 
=
�� 
ex
��  
.
��  !
Message
��! (
,
��( )
innerException
�� "
=
��# $
ex
��% '
.
��' (
InnerException
��( 6
?
��6 7
.
��7 8
Message
��8 ?
,
��? @

stackTrace
�� 
=
��  
ex
��! #
.
��# $

StackTrace
��$ .
}
�� 
)
�� 
;
�� 
}
�� 
}
�� 	
[
�� 	
	Authorize
��	 
]
�� 
[
�� 	
HttpPost
��	 
(
�� 
$str
�� #
)
��# $
]
��$ %
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
AuthResponseDto
��% 4
)
��4 5
,
��5 6
StatusCodes
��7 B
.
��B C
Status200OK
��C N
)
��N O
]
��O P
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
AuthResponseDto
��% 4
)
��4 5
,
��5 6
StatusCodes
��7 B
.
��B C!
Status400BadRequest
��C V
)
��V W
]
��W X
[
�� 	"
ProducesResponseType
��	 
(
�� 
typeof
�� $
(
��$ %
object
��% +
)
��+ ,
,
��, -
StatusCodes
��. 9
.
��9 :#
Status401Unauthorized
��: O
)
��O P
]
��P Q
public
�� 
async
�� 
Task
�� 
<
�� 
IActionResult
�� '
>
��' (
ChangePassword
��) 7
(
��7 8
[
��8 9
FromBody
��9 A
]
��A B
ChangePasswordDto
��C T
changePasswordDto
��U f
)
��f g
{
�� 	
_logger
�� 
.
�� 
LogInformation
�� "
(
��" #
LogMessages
��# .
.
��. /*
PasswordChangeRequestStarted
��/ K
)
��K L
;
��L M
var
�� 
userIdClaim
�� 
=
�� 
User
�� "
.
��" #
	FindFirst
��# ,
(
��, -

ClaimTypes
��- 7
.
��7 8
NameIdentifier
��8 F
)
��F G
;
��G H
if
�� 
(
�� 
userIdClaim
�� 
==
�� 
null
�� #
||
��$ &
!
��' (
int
��( +
.
��+ ,
TryParse
��, 4
(
��4 5
userIdClaim
��5 @
.
��@ A
Value
��A F
,
��F G
out
��H K
int
��L O
userId
��P V
)
��V W
)
��W X
{
�� 
_logger
�� 
.
�� 

LogWarning
�� "
(
��" #
LogMessages
��# .
.
��. /#
UserIdNotFoundInToken
��/ D
)
��D E
;
��E F
return
�� 
Unauthorized
�� #
(
��# $
new
��$ '
{
��( )
message
��* 1
=
��2 3
ErrorMessages
��4 A
.
��A B
InvalidUserId
��B O
}
��P Q
)
��Q R
;
��R S
}
�� 
_logger
�� 
.
�� 
LogInformation
�� "
(
��" #
string
��# )
.
��) *
Format
��* 0
(
��0 1
LogMessages
��1 <
.
��< =*
PasswordChangeRequestForUser
��= Y
,
��Y Z
userId
��[ a
)
��a b
)
��b c
;
��c d
var
�� 
result
�� 
=
�� 
await
�� 
_authService
�� +
.
��+ ,!
ChangePasswordAsync
��, ?
(
��? @
changePasswordDto
��@ Q
,
��Q R
userId
��S Y
)
��Y Z
;
��Z [
if
�� 
(
�� 
!
�� 
result
�� 
.
�� 
Success
�� 
)
��  
{
�� 
_logger
�� 
.
�� 

LogWarning
�� "
(
��" #
string
��# )
.
��) *
Format
��* 0
(
��0 1
LogMessages
��1 <
.
��< ="
PasswordChangeFailed
��= Q
,
��Q R
userId
��S Y
,
��Y Z
result
��[ a
.
��a b
ErrorMessage
��b n
)
��n o
)
��o p
;
��p q
return
�� 

BadRequest
�� !
(
��! "
result
��" (
)
��( )
;
��) *
}
�� 
_logger
�� 
.
�� 
LogInformation
�� "
(
��" #
string
��# )
.
��) *
Format
��* 0
(
��0 1
LogMessages
��1 <
.
��< =&
PasswordChangeSuccessful
��= U
,
��U V
userId
��W ]
)
��] ^
)
��^ _
;
��_ `
return
�� 
Ok
�� 
(
�� 
result
�� 
)
�� 
;
�� 
}
�� 	
}
�� 
}�� �K
UC:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\AdminController.cs
	namespace		 	
Stock		
 
.		 
API		 
.		 
Controllers		 
{

 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
[ 
	Authorize 
( 
Roles 
= 
$str 
) 
]  
public 

class 
AdminController  
:! "
ControllerBase# 1
{ 
private 
readonly  
ApplicationDbContext -
_context. 6
;6 7
private 
readonly 
ILogger  
<  !
AdminController! 0
>0 1
_logger2 9
;9 :
public 
AdminController 
(  
ApplicationDbContext 3
context4 ;
,; <
ILogger= D
<D E
AdminControllerE T
>T U
loggerV \
)\ ]
{ 	
_context 
= 
context 
; 
_logger 
= 
logger 
; 
} 	
[ 	

HttpDelete	 
( 
$str 0
)0 1
]1 2
public 
async 
Task 
< 
IActionResult '
>' ($
CleanupUnusedPermissions) A
(A B
)B C
{ 	
try 
{ 
_logger 
. 
LogInformation &
(& '
$str' U
)U V
;V W
var!! 
reportsPermission!! %
=!!& '
await!!( -
_context!!. 6
.!!6 7
Permissions!!7 B
.!!B C
FirstOrDefaultAsync!!C V
(!!V W
p!!W X
=>!!Y [
p!!\ ]
.!!] ^
Name!!^ b
==!!c e
$str!!f u
)!!u v
;!!v w
if"" 
("" 
reportsPermission"" %
!=""& (
null"") -
)""- .
{## 
await$$ 
RemovePermission$$ *
($$* +
reportsPermission$$+ <
.$$< =
Id$$= ?
,$$? @
$str$$A P
)$$P Q
;$$Q R
}%% 
var(( 
settingsPermission(( &
=((' (
await(() .
_context((/ 7
.((7 8
Permissions((8 C
.((C D
FirstOrDefaultAsync((D W
(((W X
p((X Y
=>((Z \
p((] ^
.((^ _
Name((_ c
==((d f
$str((g w
)((w x
;((x y
if)) 
()) 
settingsPermission)) &
!=))' )
null))* .
))). /
{** 
await++ 
RemovePermission++ *
(++* +
settingsPermission+++ =
.++= >
Id++> @
,++@ A
$str++B R
)++R S
;++S T
},, 
var// 
egitimPermissions// %
=//& '
await//( -
_context//. 6
.//6 7
Permissions//7 B
.//B C
Where//C H
(//H I
p//I J
=>//K M
p//N O
.//O P
Name//P T
.//T U

StartsWith//U _
(//_ `
$str//` n
)//n o
)//o p
.//p q
ToListAsync//q |
(//| }
)//} ~
;//~ 
foreach00 
(00 
var00 

permission00 '
in00( *
egitimPermissions00+ <
)00< =
{11 
await22 
RemovePermission22 *
(22* +

permission22+ 5
.225 6
Id226 8
,228 9

permission22: D
.22D E
Name22E I
)22I J
;22J K
}33 
var66 %
stockManagementPermission66 -
=66. /
await660 5
_context666 >
.66> ?
Permissions66? J
.66J K
FirstOrDefaultAsync66K ^
(66^ _
p66_ `
=>66a c
p66d e
.66e f
Name66f j
==66k m
$str	66n �
)
66� �
;
66� �
if77 
(77 %
stockManagementPermission77 -
!=77. 0
null771 5
)775 6
{88 
await99 
RemovePermission99 *
(99* +%
stockManagementPermission99+ D
.99D E
Id99E G
,99G H
$str99I `
)99` a
;99a b
}:: 
var== 
stockPermissions== $
===% &
await==' ,
_context==- 5
.==5 6
Permissions==6 A
.==A B
Where==B G
(==G H
p==H I
=>==J L
p==M N
.==N O
Group==O T
====U W
$str==X g
)==g h
.==h i
ToListAsync==i t
(==t u
)==u v
;==v w
foreach>> 
(>> 
var>> 

permission>> '
in>>( *
stockPermissions>>+ ;
)>>; <
{?? 
await@@ 
RemovePermission@@ *
(@@* +

permission@@+ 5
.@@5 6
Id@@6 8
,@@8 9

permission@@: D
.@@D E
Name@@E I
)@@I J
;@@J K
}AA 
_loggerCC 
.CC 
LogInformationCC &
(CC& '
$strCC' [
)CC[ \
;CC\ ]
returnDD 
OkDD 
(DD 
newDD 
{DD 
messageDD  '
=DD( )
$strDD* ^
}DD_ `
)DD` a
;DDa b
}EE 
catchFF 
(FF 
	ExceptionFF 
exFF 
)FF  
{GG 
_loggerHH 
.HH 
LogErrorHH  
(HH  !
exHH! #
,HH# $
$strHH% L
)HHL M
;HHM N
returnII 

StatusCodeII !
(II! "
$numII" %
,II% &
$strII' P
+IIQ R
exIIS U
.IIU V
MessageIIV ]
)II] ^
;II^ _
}JJ 
}KK 	
privateMM 
asyncMM 
TaskMM 
RemovePermissionMM +
(MM+ ,
intMM, /
permissionIdMM0 <
,MM< =
stringMM> D
permissionNameMME S
)MMS T
{NN 	
varPP 
userPermissionsPP 
=PP  !
awaitPP" '
_contextPP( 0
.PP0 1
UserPermissionsPP1 @
.QQ 
WhereQQ 
(QQ 
upQQ 
=>QQ 
upQQ 
.QQ  
PermissionIdQQ  ,
==QQ- /
permissionIdQQ0 <
)QQ< =
.RR 
ToListAsyncRR 
(RR 
)RR 
;RR 
ifTT 
(TT 
userPermissionsTT 
.TT  
AnyTT  #
(TT# $
)TT$ %
)TT% &
{UU 
_contextVV 
.VV 
RemoveRangeVV $
(VV$ %
userPermissionsVV% 4
)VV4 5
;VV5 6
_loggerWW 
.WW 
LogInformationWW &
(WW& '
$"WW' )
$strWW) *
{WW* +
permissionNameWW+ 9
}WW9 :
$strWW: I
{WWI J
userPermissionsWWJ Y
.WWY Z
CountWWZ _
}WW_ `
$strWW` {
"WW{ |
)WW| }
;WW} ~
}XX 
var[[ 
rolePermissions[[ 
=[[  !
await[[" '
_context[[( 0
.[[0 1
RolePermissions[[1 @
.\\ 
Where\\ 
(\\ 
rp\\ 
=>\\ 
rp\\ 
.\\  
PermissionId\\  ,
==\\- /
permissionId\\0 <
)\\< =
.]] 
ToListAsync]] 
(]] 
)]] 
;]] 
if__ 
(__ 
rolePermissions__ 
.__  
Any__  #
(__# $
)__$ %
)__% &
{`` 
_contextaa 
.aa 
RemoveRangeaa $
(aa$ %
rolePermissionsaa% 4
)aa4 5
;aa5 6
_loggerbb 
.bb 
LogInformationbb &
(bb& '
$"bb' )
$strbb) *
{bb* +
permissionNamebb+ 9
}bb9 :
$strbb: I
{bbI J
rolePermissionsbbJ Y
.bbY Z
CountbbZ _
}bb_ `
$strbb` u
"bbu v
)bbv w
;bbw x
}cc 
varff 

permissionff 
=ff 
awaitff "
_contextff# +
.ff+ ,
Permissionsff, 7
.ff7 8
	FindAsyncff8 A
(ffA B
permissionIdffB N
)ffN O
;ffO P
ifgg 
(gg 

permissiongg 
!=gg 
nullgg "
)gg" #
{hh 
_contextii 
.ii 
Permissionsii $
.ii$ %
Removeii% +
(ii+ ,

permissionii, 6
)ii6 7
;ii7 8
_loggerjj 
.jj 
LogInformationjj &
(jj& '
$"jj' )
$strjj) *
{jj* +
permissionNamejj+ 9
}jj9 :
$strjj: L
"jjL M
)jjM N
;jjN O
}kk 
awaitmm 
_contextmm 
.mm 
SaveChangesAsyncmm +
(mm+ ,
)mm, -
;mm- .
}nn 	
}oo 
}pp ��
[C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Controllers\ActivityLogController.cs
	namespace 	
Stock
 
. 
API 
. 
Controllers 
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public 

class !
ActivityLogController &
:' (
ControllerBase) 7
{ 
private 
readonly  
ApplicationDbContext -
_context. 6
;6 7
private 
readonly 
ILogger  
<  !!
ActivityLogController! 6
>6 7
_logger8 ?
;? @
private 
readonly 
IHostEnvironment )
_environment* 6
;6 7
public !
ActivityLogController $
($ % 
ApplicationDbContext  
context! (
,( )
ILogger 
< !
ActivityLogController )
>) *
logger+ 1
,1 2
IHostEnvironment 
environment (
)( )
{ 	
_context 
= 
context 
; 
_logger 
= 
logger 
; 
_environment 
= 
environment &
;& '
} 	
[   	
HttpGet  	 
(   
$str   
)   
]   
public!! 
async!! 
Task!! 
<!! 
IActionResult!! '
>!!' (
GetActivityLogs!!) 8
(!!8 9
[!!9 :
	FromQuery!!: C
]!!C D
int!!E H
page!!I M
=!!N O
$num!!P Q
,!!Q R
[!!S T
	FromQuery!!T ]
]!!] ^
int!!_ b
pageSize!!c k
=!!l m
$num!!n p
)!!p q
{"" 	
try## 
{$$ 
var%% 
query%% 
=%% 
_context%% $
.%%$ %
ActivityLogs%%% 1
.&& 
Include&& 
(&& 
l&& 
=>&& !
l&&" #
.&&# $
User&&$ (
)&&( )
.'' 
OrderByDescending'' &
(''& '
l''' (
=>'') +
l'', -
.''- .
	Timestamp''. 7
)''7 8
.(( 
AsQueryable((  
(((  !
)((! "
;((" #
var** 

totalItems** 
=**  
await**! &
query**' ,
.**, -

CountAsync**- 7
(**7 8
)**8 9
;**9 :
var++ 

totalPages++ 
=++  
(++! "
int++" %
)++% &
Math++& *
.++* +
Ceiling+++ 2
(++2 3

totalItems++3 =
/++> ?
(++@ A
double++A G
)++G H
pageSize++H P
)++P Q
;++Q R
var-- 
logs-- 
=-- 
await--  
query--! &
... 
Skip.. 
(.. 
(.. 
page.. 
-..  !
$num.." #
)..# $
*..% &
pageSize..' /
)../ 0
.// 
Take// 
(// 
pageSize// "
)//" #
.00 
ToListAsync00  
(00  !
)00! "
;00" #
return22 
Ok22 
(22 
new22 
{33 
Logs44 
=44 
logs44 
,44  

TotalItems55 
=55  

totalItems55! +
,55+ ,

TotalPages66 
=66  

totalPages66! +
,66+ ,
CurrentPage77 
=77  !
page77" &
}88 
)88 
;88 
}99 
catch:: 
(:: 
	Exception:: 
ex:: 
)::  
{;; 
_logger<< 
.<< 
LogError<<  
(<<  !
$"<<! #
$str<<# K
{<<K L
ex<<L N
.<<N O
Message<<O V
}<<V W
"<<W X
)<<X Y
;<<Y Z
return== 

StatusCode== !
(==! "
$num==" %
,==% &
$str==' T
)==T U
;==U V
}>> 
}?? 	
[AA 	
HttpPostAA	 
(AA 
$strAA 
)AA 
]AA 
publicBB 
asyncBB 
TaskBB 
<BB 
IActionResultBB '
>BB' (
CreateActivityLogBB) :
(BB: ;
[BB; <
FromBodyBB< D
]BBD E
objectBBF L
logDataBBM T
)BBT U
{CC 	
tryDD 
{EE 
_loggerFF 
.FF 
LogInformationFF &
(FF& '
$"FF' )
$strFF) I
"FFI J
)FFJ K
;FFK L
varII 
logJsonII 
=II 
JsonSerializerII ,
.II, -
	SerializeII- 6
(II6 7
logDataII7 >
)II> ?
;II? @
_loggerJJ 
.JJ 
LogInformationJJ &
(JJ& '
$"JJ' )
$strJJ) 5
{JJ5 6
logJsonJJ6 =
}JJ= >
"JJ> ?
)JJ? @
;JJ@ A
varMM 
logMM 
=MM 
newMM 
ActivityLogMM )
(MM) *
)MM* +
;MM+ ,
tryOO 
{PP 
varRR 

dynamicLogRR "
=RR# $
SystemRR% +
.RR+ ,
TextRR, 0
.RR0 1
JsonRR1 5
.RR5 6
JsonSerializerRR6 D
.RRD E
DeserializeRRE P
<RRP Q

DictionaryRRQ [
<RR[ \
stringRR\ b
,RRb c
JsonElementRRd o
>RRo p
>RRp q
(RRq r
logJsonRRr y
)RRy z
;RRz {
ifUU 
(UU 

dynamicLogUU "
.UU" #
TryGetValueUU# .
(UU. /
$strUU/ 7
,UU7 8
outUU9 <
varUU= @
userIdElementUUA N
)UUN O
&&UUP R
userIdElementUUS `
.UU` a
	ValueKindUUa j
!=UUk m
JsonValueKindUUn {
.UU{ |
Null	UU| �
)
UU� �
{VV 
tryWW 
{XX 
intYY 
userIdYY  &
=YY' (
userIdElementYY) 6
.YY6 7
GetInt32YY7 ?
(YY? @
)YY@ A
;YYA B
var\\ 

userExists\\  *
=\\+ ,
await\\- 2
_context\\3 ;
.\\; <
Users\\< A
.\\A B
AnyAsync\\B J
(\\J K
u\\K L
=>\\M O
u\\P Q
.\\Q R
Id\\R T
==\\U W
userId\\X ^
)\\^ _
;\\_ `
if^^ 
(^^  

userExists^^  *
)^^* +
{__ 
log``  #
.``# $
UserId``$ *
=``+ ,
userId``- 3
;``3 4
}aa 
elsebb  
{cc 
_loggerdd  '
.dd' (

LogWarningdd( 2
(dd2 3
$"dd3 5
$strdd5 H
{ddH I
userIdddI O
}ddO P
$str	ddP �
"
dd� �
)
dd� �
;
dd� �
logee  #
.ee# $
UserIdee$ *
=ee+ ,
nullee- 1
;ee1 2
}ff 
}gg 
catchhh 
(hh 
	Exceptionhh (
exhh) +
)hh+ ,
{ii 
_loggerjj #
.jj# $

LogWarningjj$ .
(jj. /
$"jj/ 1
$strjj1 U
{jjU V
exjjV X
.jjX Y
MessagejjY `
}jj` a
$str	jja �
"
jj� �
)
jj� �
;
jj� �
logkk 
.kk  
UserIdkk  &
=kk' (
nullkk) -
;kk- .
}ll 
}mm 
elsenn 
{oo 
_loggerpp 
.pp  

LogWarningpp  *
(pp* +
$strpp+ v
)ppv w
;ppw x
logqq 
.qq 
UserIdqq "
=qq# $
nullqq% )
;qq) *
}rr 
iftt 
(tt 

dynamicLogtt "
.tt" #
TryGetValuett# .
(tt. /
$strtt/ 9
,tt9 :
outtt; >
vartt? B
usernameElementttC R
)ttR S
&&ttT V
usernameElementttW f
.ttf g
	ValueKindttg p
!=ttq s
JsonValueKind	ttt �
.
tt� �
Null
tt� �
)
tt� �
{uu 
logvv 
.vv 
Usernamevv $
=vv% &
usernameElementvv' 6
.vv6 7
	GetStringvv7 @
(vv@ A
)vvA B
??vvC E
$strvvF M
;vvM N
}ww 
elsexx 
{yy 
logzz 
.zz 
Usernamezz $
=zz% &
$strzz' .
;zz. /
}{{ 
if}} 
(}} 

dynamicLog}} "
.}}" #
TryGetValue}}# .
(}}. /
$str}}/ =
,}}= >
out}}? B
var}}C F
activityTypeElement}}G Z
)}}Z [
&&}}\ ^
activityTypeElement}}_ r
.}}r s
	ValueKind}}s |
!=}}} 
JsonValueKind
}}� �
.
}}� �
Null
}}� �
)
}}� �
{~~ 
log 
. 
ActivityType (
=) *
activityTypeElement+ >
.> ?
	GetString? H
(H I
)I J
??K M
$strN W
;W X
}
�� 
else
�� 
{
�� 
log
�� 
.
�� 
ActivityType
�� (
=
��) *
$str
��+ 4
;
��4 5
}
�� 
if
�� 
(
�� 

dynamicLog
�� "
.
��" #
TryGetValue
��# .
(
��. /
$str
��/ <
,
��< =
out
��> A
var
��B E 
descriptionElement
��F X
)
��X Y
&&
��Z \ 
descriptionElement
��] o
.
��o p
	ValueKind
��p y
!=
��z |
JsonValueKind��} �
.��� �
Null��� �
)��� �
{
�� 
log
�� 
.
�� 
Description
�� '
=
��( ) 
descriptionElement
��* <
.
��< =
	GetString
��= F
(
��F G
)
��G H
??
��I K
$str
��L \
;
��\ ]
}
�� 
else
�� 
{
�� 
log
�� 
.
�� 
Description
�� '
=
��( )
$str
��* :
;
��: ;
}
�� 
if
�� 
(
�� 

dynamicLog
�� "
.
��" #
TryGetValue
��# .
(
��. /
$str
��/ :
,
��: ;
out
��< ?
var
��@ C
timestampElement
��D T
)
��T U
&&
��V X
timestampElement
��Y i
.
��i j
	ValueKind
��j s
!=
��t v
JsonValueKind��w �
.��� �
Null��� �
)��� �
{
�� 
try
�� 
{
�� 
log
�� 
.
��  
	Timestamp
��  )
=
��* +
timestampElement
��, <
.
��< =
GetDateTime
��= H
(
��H I
)
��I J
;
��J K
}
�� 
catch
�� 
(
�� 
	Exception
�� (
ex
��) +
)
��+ ,
{
�� 
_logger
�� #
.
��# $

LogWarning
��$ .
(
��. /
$"
��/ 1
$str
��1 ]
{
��] ^
ex
��^ `
.
��` a
Message
��a h
}
��h i
$str��i �
"��� �
)��� �
;��� �
log
�� 
.
��  
	Timestamp
��  )
=
��* +
DateTime
��, 4
.
��4 5
UtcNow
��5 ;
;
��; <
}
�� 
}
�� 
else
�� 
{
�� 
log
�� 
.
�� 
	Timestamp
�� %
=
��& '
DateTime
��( 0
.
��0 1
UtcNow
��1 7
;
��7 8
}
�� 
var
�� 
additionalInfo
�� &
=
��' (
new
��) ,

Dictionary
��- 7
<
��7 8
string
��8 >
,
��> ?
object
��@ F
>
��F G
(
��G H
)
��H I
;
��I J
if
�� 
(
�� 

dynamicLog
�� "
.
��" #
TryGetValue
��# .
(
��. /
$str
��/ 7
,
��7 8
out
��9 <
var
��= @
statusElement
��A N
)
��N O
&&
��P R
statusElement
��S `
.
��` a
	ValueKind
��a j
!=
��k m
JsonValueKind
��n {
.
��{ |
Null��| �
)��� �
{
�� 
additionalInfo
�� &
[
��& '
$str
��' /
]
��/ 0
=
��1 2
statusElement
��3 @
.
��@ A
	GetString
��A J
(
��J K
)
��K L
??
��M O
$str
��P V
;
��V W
}
�� 
if
�� 
(
�� 

dynamicLog
�� "
.
��" #
TryGetValue
��# .
(
��. /
$str
��/ 8
,
��8 9
out
��: =
var
��> A
detailsElement
��B P
)
��P Q
&&
��R T
detailsElement
��U c
.
��c d
	ValueKind
��d m
!=
��n p
JsonValueKind
��q ~
.
��~ 
Null�� �
)��� �
{
�� 
additionalInfo
�� &
[
��& '
$str
��' 0
]
��0 1
=
��2 3
detailsElement
��4 B
.
��B C
ToString
��C K
(
��K L
)
��L M
;
��M N
}
�� 
if
�� 
(
�� 

dynamicLog
�� "
.
��" #
TryGetValue
��# .
(
��. /
$str
��/ :
,
��: ;
out
��< ?
var
��@ C
ipAddressElement
��D T
)
��T U
&&
��V X
ipAddressElement
��Y i
.
��i j
	ValueKind
��j s
!=
��t v
JsonValueKind��w �
.��� �
Null��� �
)��� �
{
�� 
additionalInfo
�� &
[
��& '
$str
��' 2
]
��2 3
=
��4 5
ipAddressElement
��6 F
.
��F G
	GetString
��G P
(
��P Q
)
��Q R
??
��S U
string
��V \
.
��\ ]
Empty
��] b
;
��b c
}
�� 
log
�� 
.
�� 
AdditionalInfo
�� &
=
��' (
JsonSerializer
��) 7
.
��7 8
	Serialize
��8 A
(
��A B
additionalInfo
��B P
)
��P Q
;
��Q R
}
�� 
catch
�� 
(
�� 
JsonException
�� $
jsonEx
��% +
)
��+ ,
{
�� 
_logger
�� 
.
�� 
LogError
�� $
(
��$ %
$"
��% '
$str
��' <
{
��< =
jsonEx
��= C
.
��C D
Message
��D K
}
��K L
"
��L M
)
��M N
;
��N O
return
�� 

BadRequest
�� %
(
��% &
$"
��& (
$str
��( =
{
��= >
jsonEx
��> D
.
��D E
Message
��E L
}
��L M
"
��M N
)
��N O
;
��O P
}
�� 
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
$"
��' )
$str
��) P
{
��P Q
log
��Q T
.
��T U
UserId
��U [
}
��[ \
$str
��\ l
{
��l m
log
��m p
.
��p q
ActivityType
��q }
}
��} ~
"
��~ 
)�� �
;��� �
_context
�� 
.
�� 
ActivityLogs
�� %
.
��% &
Add
��& )
(
��) *
log
��* -
)
��- .
;
��. /
await
�� 
_context
�� 
.
�� 
SaveChangesAsync
�� /
(
��/ 0
)
��0 1
;
��1 2
return
�� 
Ok
�� 
(
�� 
new
�� 
{
�� 
message
��  '
=
��( )
$str
��* D
,
��D E
log
��F I
}
��J K
)
��K L
;
��L M
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
$"
��! #
$str
��# L
{
��L M
ex
��M O
.
��O P
Message
��P W
}
��W X
"
��X Y
)
��Y Z
;
��Z [
_logger
�� 
.
�� 
LogError
��  
(
��  !
$"
��! #
$str
��# ,
{
��, -
ex
��- /
.
��/ 0
InnerException
��0 >
?
��> ?
.
��? @
Message
��@ G
}
��G H
"
��H I
)
��I J
;
��J K
_logger
�� 
.
�� 
LogError
��  
(
��  !
$"
��! #
$str
��# 0
{
��0 1
ex
��1 3
.
��3 4

StackTrace
��4 >
}
��> ?
"
��? @
)
��@ A
;
��A B
if
�� 
(
�� 
_environment
��  
.
��  !
IsDevelopment
��! .
(
��. /
)
��/ 0
)
��0 1
{
�� 
return
�� 

StatusCode
�� %
(
��% &
$num
��& )
,
��) *
new
��+ .
{
�� 
error
�� 
=
�� 
$str
��  N
,
��N O
message
�� 
=
��  !
ex
��" $
.
��$ %
Message
��% ,
,
��, -
innerException
�� &
=
��' (
ex
��) +
.
��+ ,
InnerException
��, :
?
��: ;
.
��; <
Message
��< C
,
��C D

stackTrace
�� "
=
��# $
ex
��% '
.
��' (

StackTrace
��( 2
}
�� 
)
�� 
;
�� 
}
�� 
return
�� 

StatusCode
�� !
(
��! "
$num
��" %
,
��% &
$str
��' U
)
��U V
;
��V W
}
�� 
}
�� 	
[
�� 	
HttpPost
��	 
(
�� 
$str
�� !
)
��! "
]
��" #
public
�� 
async
�� 
Task
�� 
<
�� 
IActionResult
�� '
>
��' ($
CreateBulkActivityLogs
��) ?
(
��? @
[
��@ A
FromBody
��A I
]
��I J
List
��K O
<
��O P
object
��P V
>
��V W
logsData
��X `
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
�� 
LogInformation
�� &
(
��& '
$"
��' )
$str
��) Z
{
��Z [
logsData
��[ c
.
��c d
Count
��d i
}
��i j
"
��j k
)
��k l
;
��l m
_logger
�� 
.
�� 
LogInformation
�� &
(
��& '
$"
��' )
$str
��) 5
{
��5 6
JsonSerializer
��6 D
.
��D E
	Serialize
��E N
(
��N O
logsData
��O W
)
��W X
}
��X Y
"
��Y Z
)
��Z [
;
��[ \
List
�� 
<
�� 
ActivityLog
��  
>
��  !
logs
��" &
=
��' (
new
��) ,
List
��- 1
<
��1 2
ActivityLog
��2 =
>
��= >
(
��> ?
)
��? @
;
��@ A
foreach
�� 
(
�� 
var
�� 
logData
�� $
in
��% '
logsData
��( 0
)
��0 1
{
�� 
try
�� 
{
�� 
var
�� 
logJson
�� #
=
��$ %
JsonSerializer
��& 4
.
��4 5
	Serialize
��5 >
(
��> ?
logData
��? F
)
��F G
;
��G H
_logger
�� 
.
��  
LogInformation
��  .
(
��. /
$"
��/ 1
$str
��1 >
{
��> ?
logJson
��? F
}
��F G
"
��G H
)
��H I
;
��I J
var
�� 
log
�� 
=
��  !
new
��" %
ActivityLog
��& 1
(
��1 2
)
��2 3
;
��3 4
try
�� 
{
�� 
var
�� 

dynamicLog
��  *
=
��+ ,
System
��- 3
.
��3 4
Text
��4 8
.
��8 9
Json
��9 =
.
��= >
JsonSerializer
��> L
.
��L M
Deserialize
��M X
<
��X Y

Dictionary
��Y c
<
��c d
string
��d j
,
��j k
JsonElement
��l w
>
��w x
>
��x y
(
��y z
logJson��z �
)��� �
;��� �
if
�� 
(
��  

dynamicLog
��  *
.
��* +
TryGetValue
��+ 6
(
��6 7
$str
��7 ?
,
��? @
out
��A D
var
��E H
userIdElement
��I V
)
��V W
&&
��X Z
userIdElement
��[ h
.
��h i
	ValueKind
��i r
!=
��s u
JsonValueKind��v �
.��� �
Null��� �
)��� �
{
�� 
try
��  #
{
��  !
int
��$ '
userId
��( .
=
��/ 0
userIdElement
��1 >
.
��> ?
GetInt32
��? G
(
��G H
)
��H I
;
��I J
var
��$ '

userExists
��( 2
=
��3 4
await
��5 :
_context
��; C
.
��C D
Users
��D I
.
��I J
AnyAsync
��J R
(
��R S
u
��S T
=>
��U W
u
��X Y
.
��Y Z
Id
��Z \
==
��] _
userId
��` f
)
��f g
;
��g h
if
��$ &
(
��' (

userExists
��( 2
)
��2 3
{
��$ %
log
��( +
.
��+ ,
UserId
��, 2
=
��3 4
userId
��5 ;
;
��; <
}
��$ %
else
��$ (
{
��$ %
_logger
��( /
.
��/ 0

LogWarning
��0 :
(
��: ;
$"
��; =
$str
��= P
{
��P Q
userId
��Q W
}
��W X
$str��X �
"��� �
)��� �
;��� �
log
��( +
.
��+ ,
UserId
��, 2
=
��3 4
null
��5 9
;
��9 :
}
��$ %
}
��  !
catch
��  %
(
��& '
	Exception
��' 0
ex
��1 3
)
��3 4
{
��  !
_logger
��$ +
.
��+ ,

LogWarning
��, 6
(
��6 7
$"
��7 9
$str
��9 ]
{
��] ^
ex
��^ `
.
��` a
Message
��a h
}
��h i
$str��i �
"��� �
)��� �
;��� �
log
��$ '
.
��' (
UserId
��( .
=
��/ 0
null
��1 5
;
��5 6
}
��  !
}
�� 
else
��  
{
�� 
_logger
��  '
.
��' (

LogWarning
��( 2
(
��2 3
$str
��3 ~
)
��~ 
;�� �
log
��  #
.
��# $
UserId
��$ *
=
��+ ,
null
��- 1
;
��1 2
}
�� 
if
�� 
(
��  

dynamicLog
��  *
.
��* +
TryGetValue
��+ 6
(
��6 7
$str
��7 A
,
��A B
out
��C F
var
��G J
usernameElement
��K Z
)
��Z [
&&
��\ ^
usernameElement
��_ n
.
��n o
	ValueKind
��o x
!=
��y {
JsonValueKind��| �
.��� �
Null��� �
)��� �
{
�� 
log
��  #
.
��# $
Username
��$ ,
=
��- .
usernameElement
��/ >
.
��> ?
	GetString
��? H
(
��H I
)
��I J
??
��K M
$str
��N U
;
��U V
}
�� 
else
��  
{
�� 
log
��  #
.
��# $
Username
��$ ,
=
��- .
$str
��/ 6
;
��6 7
}
�� 
if
�� 
(
��  

dynamicLog
��  *
.
��* +
TryGetValue
��+ 6
(
��6 7
$str
��7 E
,
��E F
out
��G J
var
��K N!
activityTypeElement
��O b
)
��b c
&&
��d f!
activityTypeElement
��g z
.
��z {
	ValueKind��{ �
!=��� �
JsonValueKind��� �
.��� �
Null��� �
)��� �
{
�� 
log
��  #
.
��# $
ActivityType
��$ 0
=
��1 2!
activityTypeElement
��3 F
.
��F G
	GetString
��G P
(
��P Q
)
��Q R
??
��S U
$str
��V _
;
��_ `
}
�� 
else
��  
{
�� 
log
��  #
.
��# $
ActivityType
��$ 0
=
��1 2
$str
��3 <
;
��< =
}
�� 
if
�� 
(
��  

dynamicLog
��  *
.
��* +
TryGetValue
��+ 6
(
��6 7
$str
��7 D
,
��D E
out
��F I
var
��J M 
descriptionElement
��N `
)
��` a
&&
��b d 
descriptionElement
��e w
.
��w x
	ValueKind��x �
!=��� �
JsonValueKind��� �
.��� �
Null��� �
)��� �
{
�� 
log
��  #
.
��# $
Description
��$ /
=
��0 1 
descriptionElement
��2 D
.
��D E
	GetString
��E N
(
��N O
)
��O P
??
��Q S
$str
��T d
;
��d e
}
�� 
else
��  
{
�� 
log
��  #
.
��# $
Description
��$ /
=
��0 1
$str
��2 B
;
��B C
}
�� 
if
�� 
(
��  

dynamicLog
��  *
.
��* +
TryGetValue
��+ 6
(
��6 7
$str
��7 B
,
��B C
out
��D G
var
��H K
timestampElement
��L \
)
��\ ]
&&
��^ `
timestampElement
��a q
.
��q r
	ValueKind
��r {
!=
��| ~
JsonValueKind�� �
.��� �
Null��� �
)��� �
{
�� 
try
��  #
{
��  !
log
��$ '
.
��' (
	Timestamp
��( 1
=
��2 3
timestampElement
��4 D
.
��D E
GetDateTime
��E P
(
��P Q
)
��Q R
;
��R S
}
��  !
catch
��  %
(
��& '
	Exception
��' 0
ex
��1 3
)
��3 4
{
��  !
_logger
��$ +
.
��+ ,

LogWarning
��, 6
(
��6 7
$"
��7 9
$str
��9 e
{
��e f
ex
��f h
.
��h i
Message
��i p
}
��p q
$str��q �
"��� �
)��� �
;��� �
log
��$ '
.
��' (
	Timestamp
��( 1
=
��2 3
DateTime
��4 <
.
��< =
UtcNow
��= C
;
��C D
}
��  !
}
�� 
else
��  
{
�� 
log
��  #
.
��# $
	Timestamp
��$ -
=
��. /
DateTime
��0 8
.
��8 9
UtcNow
��9 ?
;
��? @
}
�� 
var
�� 
additionalInfo
��  .
=
��/ 0
new
��1 4

Dictionary
��5 ?
<
��? @
string
��@ F
,
��F G
object
��H N
>
��N O
(
��O P
)
��P Q
;
��Q R
if
�� 
(
��  

dynamicLog
��  *
.
��* +
TryGetValue
��+ 6
(
��6 7
$str
��7 ?
,
��? @
out
��A D
var
��E H
statusElement
��I V
)
��V W
&&
��X Z
statusElement
��[ h
.
��h i
	ValueKind
��i r
!=
��s u
JsonValueKind��v �
.��� �
Null��� �
)��� �
{
�� 
additionalInfo
��  .
[
��. /
$str
��/ 7
]
��7 8
=
��9 :
statusElement
��; H
.
��H I
	GetString
��I R
(
��R S
)
��S T
??
��U W
$str
��X ^
;
��^ _
}
�� 
if
�� 
(
��  

dynamicLog
��  *
.
��* +
TryGetValue
��+ 6
(
��6 7
$str
��7 @
,
��@ A
out
��B E
var
��F I
detailsElement
��J X
)
��X Y
&&
��Z \
detailsElement
��] k
.
��k l
	ValueKind
��l u
!=
��v x
JsonValueKind��y �
.��� �
Null��� �
)��� �
{
�� 
additionalInfo
��  .
[
��. /
$str
��/ 8
]
��8 9
=
��: ;
detailsElement
��< J
.
��J K
ToString
��K S
(
��S T
)
��T U
;
��U V
}
�� 
if
�� 
(
��  

dynamicLog
��  *
.
��* +
TryGetValue
��+ 6
(
��6 7
$str
��7 B
,
��B C
out
��D G
var
��H K
ipAddressElement
��L \
)
��\ ]
&&
��^ `
ipAddressElement
��a q
.
��q r
	ValueKind
��r {
!=
��| ~
JsonValueKind�� �
.��� �
Null��� �
)��� �
{
�� 
additionalInfo
��  .
[
��. /
$str
��/ :
]
��: ;
=
��< =
ipAddressElement
��> N
.
��N O
	GetString
��O X
(
��X Y
)
��Y Z
??
��[ ]
string
��^ d
.
��d e
Empty
��e j
;
��j k
}
�� 
log
�� 
.
��  
AdditionalInfo
��  .
=
��/ 0
JsonSerializer
��1 ?
.
��? @
	Serialize
��@ I
(
��I J
additionalInfo
��J X
)
��X Y
;
��Y Z
}
�� 
catch
�� 
(
�� 
JsonException
�� ,
jsonEx
��- 3
)
��3 4
{
�� 
_logger
�� #
.
��# $
LogError
��$ ,
(
��, -
$"
��- /
$str
��/ D
{
��D E
jsonEx
��E K
.
��K L
Message
��L S
}
��S T
$str
��T d
"
��d e
)
��e f
;
��f g
continue
�� $
;
��$ %
}
�� 
logs
�� 
.
�� 
Add
��  
(
��  !
log
��! $
)
��$ %
;
��% &
}
�� 
catch
�� 
(
�� 
	Exception
�� $
ex
��% '
)
��' (
{
�� 
_logger
�� 
.
��  
LogError
��  (
(
��( )
$"
��) +
$str
��+ Q
{
��Q R
ex
��R T
.
��T U
Message
��U \
}
��\ ]
$str
��] _
{
��_ `
ex
��` b
.
��b c

StackTrace
��c m
}
��m n
"
��n o
)
��o p
;
��p q
}
�� 
}
�� 
if
�� 
(
�� 
logs
�� 
.
�� 
Count
�� 
>
��  
$num
��! "
)
��" #
{
�� 
_logger
�� 
.
�� 
LogInformation
�� *
(
��* +
$"
��+ -
$str
��- :
{
��: ;
logs
��; ?
.
��? @
Count
��@ E
}
��E F
$str
��F \
"
��\ ]
)
��] ^
;
��^ _
_context
�� 
.
�� 
ActivityLogs
�� )
.
��) *
AddRange
��* 2
(
��2 3
logs
��3 7
)
��7 8
;
��8 9
await
�� 
_context
�� "
.
��" #
SaveChangesAsync
��# 3
(
��3 4
)
��4 5
;
��5 6
return
�� 
Ok
�� 
(
�� 
new
�� !
{
��" #
message
��$ +
=
��, -
$"
��. 0
{
��0 1
logs
��1 5
.
��5 6
Count
��6 ;
}
��; <
$str
��< Z
"
��Z [
,
��[ \
logs
��] a
}
��b c
)
��c d
;
��d e
}
�� 
else
�� 
{
�� 
_logger
�� 
.
�� 

LogWarning
�� &
(
��& '
$str
��' E
)
��E F
;
��F G
return
�� 

BadRequest
�� %
(
��% &
$str
��& {
)
��{ |
;
��| }
}
�� 
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
$"
��! #
$str
��# U
{
��U V
ex
��V X
.
��X Y
Message
��Y `
}
��` a
"
��a b
)
��b c
;
��c d
_logger
�� 
.
�� 
LogError
��  
(
��  !
$"
��! #
$str
��# ,
{
��, -
ex
��- /
.
��/ 0
InnerException
��0 >
?
��> ?
.
��? @
Message
��@ G
}
��G H
"
��H I
)
��I J
;
��J K
_logger
�� 
.
�� 
LogError
��  
(
��  !
$"
��! #
$str
��# 0
{
��0 1
ex
��1 3
.
��3 4

StackTrace
��4 >
}
��> ?
"
��? @
)
��@ A
;
��A B
if
�� 
(
�� 
_environment
��  
.
��  !
IsDevelopment
��! .
(
��. /
)
��/ 0
)
��0 1
{
�� 
return
�� 

StatusCode
�� %
(
��% &
$num
��& )
,
��) *
new
��+ .
{
�� 
error
�� 
=
�� 
$str
��  W
,
��W X
message
�� 
=
��  !
ex
��" $
.
��$ %
Message
��% ,
,
��, -
innerException
�� &
=
��' (
ex
��) +
.
��+ ,
InnerException
��, :
?
��: ;
.
��; <
Message
��< C
,
��C D

stackTrace
�� "
=
��# $
ex
��% '
.
��' (

StackTrace
��( 2
}
�� 
)
�� 
;
�� 
}
�� 
return
�� 

StatusCode
�� !
(
��! "
$num
��" %
,
��% &
$str
��' ^
)
��^ _
;
��_ `
}
�� 
}
�� 	
}
�� 
}�� �
_C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.API\Attributes\RequirePermissionAttribute.cs
	namespace 	
Stock
 
. 
API 
. 

Attributes 
{		 
[

 
AttributeUsage

 
(

 
AttributeTargets

 $
.

$ %
Method

% +
|

, -
AttributeTargets

. >
.

> ?
Class

? D
)

D E
]

E F
public 

class &
RequirePermissionAttribute +
:, -
	Attribute. 7
,7 8
IAsyncActionFilter9 K
{ 
private 
readonly 
string 
_permission  +
;+ ,
public &
RequirePermissionAttribute )
() *
string* 0

permission1 ;
); <
{ 	
_permission 
= 

permission $
;$ %
} 	
public 
async 
Task "
OnActionExecutionAsync 0
(0 1"
ActionExecutingContext1 G
contextH O
,O P#
ActionExecutionDelegateQ h
nexti m
)m n
{ 	
var 
permissionService !
=" #
context$ +
.+ ,
HttpContext, 7
.7 8
RequestServices8 G
. 
GetRequiredService #
<# $
IPermissionService$ 6
>6 7
(7 8
)8 9
;9 :
var 
currentUserService "
=# $
context% ,
., -
HttpContext- 8
.8 9
RequestServices9 H
. 
GetRequiredService #
<# $
ICurrentUserService$ 7
>7 8
(8 9
)9 :
;: ;
if 
( 
currentUserService "
." #
UserId# )
==* ,
null- 1
)1 2
{ 
context 
. 
Result 
=  
new! $
UnauthorizedResult% 7
(7 8
)8 9
;9 :
return 
; 
} 
bool!! 
hasPermission!! 
=!!  
await!!! &
permissionService!!' 8
.!!8 9"
UserHasPermissionAsync!!9 O
(!!O P
currentUserService"" "
.""" #
UserId""# )
."") *
Value""* /
,""/ 0
_permission""1 <
)""< =
;""= >
if$$ 
($$ 
!$$ 
hasPermission$$ 
)$$ 
{%% 
context&& 
.&& 
Result&& 
=&&  
new&&! $
ForbidResult&&% 1
(&&1 2
)&&2 3
;&&3 4
return'' 
;'' 
}(( 
await** 
next** 
(** 
)** 
;** 
}++ 	
},, 
}-- 