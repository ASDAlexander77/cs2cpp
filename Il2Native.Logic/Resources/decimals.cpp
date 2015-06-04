// Decimals

typedef union {
	UInt64 int64;
	struct {
#if BIGENDIAN
		UInt32 Hi;
		UInt32 Lo;
#else            
		UInt32 Lo;
		UInt32 Hi;
#endif           
	} u;
} SPLIT64;

typedef union{
	struct {
#if BIGENDIAN
		UInt32 sign : 1;
		UInt32 exp : 11;
		UInt32 mantHi : 20;
		UInt32 mantLo;
#else // BIGENDIAN
		UInt32 mantLo;
		UInt32 mantHi : 20;
		UInt32 exp : 11;
		UInt32 sign : 1;
#endif
	} u;
	double dbl;
} DBLSTRUCT;

typedef struct {
#if BIGENDIAN
	UInt32 sign : 1;
	UInt32 exp : 8;
	UInt32 mant : 23;
#else
	UInt32 mant : 23;
	UInt32 exp : 8;
	UInt32 sign : 1;
#endif
} SNGSTRUCT;

#define OVFL_MAX_1_HI   429496729
#define DEC_SCALE_MAX   28
#define POWER10_MAX 9

#define OVFL_MAX_9_HI   4u
#define OVFL_MAX_9_MID  1266874889u
#define OVFL_MAX_9_LO   3047500985u

#define OVFL_MAX_5_HI   42949

#define DECIMAL_NEG ((Byte)0x80)
#define DECIMAL_SCALE(dec)       ((dec).u.u.scale)
#define DECIMAL_SIGN(dec)        ((dec).u.u.sign)
#define DECIMAL_SIGNSCALE(dec)   ((dec).u.signscale)
#define DECIMAL_LO32(dec)        ((dec).v.v.Lo32)
#define DECIMAL_MID32(dec)       ((dec).v.v.Mid32)
#define DECIMAL_HI32(dec)        ((dec).Hi32)

#define DECIMAL_LO64_GET(dec)       ((UInt64)(((UInt64)DECIMAL_MID32(dec) << 32) | DECIMAL_LO32(dec)))
#define DECIMAL_LO64_SET(dec,value)   {UInt64 _Value = value; DECIMAL_MID32(dec) = (UInt32)(_Value >> 32); DECIMAL_LO32(dec) = (UInt32)_Value; }
#define DECIMAL_SETZERO(dec) {DECIMAL_LO32(dec) = 0; DECIMAL_MID32(dec) = 0; DECIMAL_HI32(dec) = 0; DECIMAL_SIGNSCALE(dec) = 0;}

const SPLIT64    sdlTenToEighteen = { UInt64(1000000000000000000) };

#define NOERROR 0
#define DISP_E_OVERFLOW 1
#define DISP_E_DIVBYZERO 2

typedef union tagCY {
	struct {
#if BIGENDIAN
		Int32    Hi;
		UInt32   Lo;
#else
		UInt32   Lo;
		Int32    Hi;
#endif
	} u;
	Int64 int64;
} CY, *LPCY;

typedef CY CURRENCY;

typedef struct tagDEC {

	// Decimal.cs treats the first two shorts as one long
	// And they seriable the data so we need to little endian
	// seriliazation
	// The wReserved overlaps with Variant's vt member
#if BIGENDIAN
	union {
		struct {
			Byte sign;
			Byte scale;
		} u;
		UInt16 signscale;
	} u;
	UInt16 wReserved;
#else
	UInt16 wReserved;
	union {
		struct {
			Byte scale;
			Byte sign;
		} u;
		UInt16 signscale;
	} u;
#endif
	UInt32 Hi32;
	union {
		struct {
			UInt32 Lo32;
			UInt32 Mid32;
		} v;
		// Don't use Lo64 in the PAL: UInt64 Lo64;
	} v;
} DECIMAL;


UInt32 ulPower10[POWER10_MAX + 1] = { 1, 10, 100, 1000, 10000, 100000, 1000000,
10000000, 100000000, 1000000000 };

const SPLIT64 sdlPower10[] = { { UInt64(10000000000) },          // 1E10
{ UInt64(100000000000) },     // 1E11
{ UInt64(1000000000000) },    // 1E12
{ UInt64(10000000000000) },   // 1E13
{ UInt64(100000000000000) } }; // 1E14

struct DECOVFL
{
	UInt32 Hi;
	UInt32 Mid;
	UInt32 Lo;
};

DECOVFL PowerOvfl[] = {
	// This is a table of the largest values that can be in the upper two
	// Int32s of a 96-bit number that will not overflow when multiplied
	// by a given power.  For the upper word, this is a table of 
	// 2^32 / 10^n for 1 <= n <= 9.  For the lower word, this is the
	// remaining fraction part * 2^32.  2^32 = 4294967296.
	// 
	{ 429496729u, 2576980377u, 2576980377u }, // 10^1 remainder 0.6
	{ 42949672u, 4123168604u, 687194767u }, // 10^2 remainder 0.16
	{ 4294967u, 1271310319u, 2645699854u }, // 10^3 remainder 0.616
	{ 429496u, 3133608139u, 694066715u }, // 10^4 remainder 0.1616
	{ 42949u, 2890341191u, 2216890319u }, // 10^5 remainder 0.51616
	{ 4294u, 4154504685u, 2369172679u }, // 10^6 remainder 0.551616
	{ 429u, 2133437386u, 4102387834u }, // 10^7 remainder 0.9551616
	{ 42u, 4078814305u, 410238783u }, // 10^8 remainder 0.09991616
	{ 4u, 1266874889u, 3047500985u }, // 10^9 remainder 0.709551616
};

const Double dblPower10[] = {
	1, 1e1, 1e2, 1e3, 1e4, 1e5, 1e6, 1e7, 1e8, 1e9,
	1e10, 1e11, 1e12, 1e13, 1e14, 1e15, 1e16, 1e17, 1e18, 1e19,
	1e20, 1e21, 1e22, 1e23, 1e24, 1e25, 1e26, 1e27, 1e28, 1e29,
	1e30, 1e31, 1e32, 1e33, 1e34, 1e35, 1e36, 1e37, 1e38, 1e39,
	1e40, 1e41, 1e42, 1e43, 1e44, 1e45, 1e46, 1e47, 1e48, 1e49,
	1e50, 1e51, 1e52, 1e53, 1e54, 1e55, 1e56, 1e57, 1e58, 1e59,
	1e60, 1e61, 1e62, 1e63, 1e64, 1e65, 1e66, 1e67, 1e68, 1e69,
	1e70, 1e71, 1e72, 1e73, 1e74, 1e75, 1e76, 1e77, 1e78, 1e79,
	1e80 };


#define DBLBIAS 1022

#define SNGBIAS 126

#define DEC_MAXDIG 29
#define DECMAX 28

#define MAXINTPWR10 22

static const UInt32 ulTenToTenDiv4 = 2500000000U;
static const UInt32 ulTenToNine = 1000000000;
#define COPYDEC(dest, src) {DECIMAL_SIGNSCALE(dest) = DECIMAL_SIGNSCALE(src); DECIMAL_HI32(dest) = DECIMAL_HI32(src); DECIMAL_LO64_SET(dest, DECIMAL_LO64_GET(src));}

#define UInt32x32To64(a, b) ((UInt64)((UInt32)(a)) * (UInt64)((UInt32)(b)))

#define Div64by32(num, den) ((UInt32)((UInt64)(num) / (UInt32)(den)))
#define Mod64by32(num, den) ((UInt32)((UInt64)(num) % (UInt32)(den)))

template <class T> const T& min(const T& a, const T& b)
{
	return !(b < a) ? a : b;     // or: return !comp(b,a)?a:b; for version (2)
}

Double fnDblPower10(Int32 ix)
{
	const Int32 maxIx = (sizeof(dblPower10) / sizeof(dblPower10[0]));
	if (ix < maxIx)
		return dblPower10[ix];
	return pow(10.0, ix);
} // Double fnDblPower10()

inline UInt64 DivMod64by32(UInt64 num, UInt32 den)
{
	SPLIT64  sdl;

	sdl.u.Lo = Div64by32(num, den);
	sdl.u.Hi = Mod64by32(num, den);
	return sdl.int64;
}

inline UInt64 DivMod32by32(UInt32 num, UInt32 den)
{
	SPLIT64  sdl;

	sdl.u.Lo = num / den;
	sdl.u.Hi = num % den;
	return sdl.int64;
}

UInt64 UInt64x64To128(SPLIT64 sdlOp1, SPLIT64 sdlOp2, UInt64 *pdlHi)
{
	SPLIT64  sdlTmp1;
	SPLIT64  sdlTmp2;
	SPLIT64  sdlTmp3;

	sdlTmp1.int64 = UInt32x32To64(sdlOp1.u.Lo, sdlOp2.u.Lo); // lo partial prod
	sdlTmp2.int64 = UInt32x32To64(sdlOp1.u.Lo, sdlOp2.u.Hi); // mid 1 partial prod
	sdlTmp1.u.Hi += sdlTmp2.u.Lo;
	if (sdlTmp1.u.Hi < sdlTmp2.u.Lo)  // test for carry
		sdlTmp2.u.Hi++;
	sdlTmp3.int64 = UInt32x32To64(sdlOp1.u.Hi, sdlOp2.u.Hi) + (UInt64)sdlTmp2.u.Hi;
	sdlTmp2.int64 = UInt32x32To64(sdlOp1.u.Hi, sdlOp2.u.Lo);
	sdlTmp1.u.Hi += sdlTmp2.u.Lo;
	if (sdlTmp1.u.Hi < sdlTmp2.u.Lo)  // test for carry
		sdlTmp2.u.Hi++;
	sdlTmp3.int64 += (UInt64)sdlTmp2.u.Hi;

	*pdlHi = sdlTmp3.int64;
	return sdlTmp1.int64;
}

UInt32 FullDiv64By32(UInt64 *pdlNum, UInt32 ulDen)
{
	SPLIT64  sdlTmp;
	SPLIT64  sdlRes;

	sdlTmp.int64 = *pdlNum;
	sdlRes.u.Hi = 0;

	if (sdlTmp.u.Hi >= ulDen) {
		// DivMod64by32 returns quotient in Lo, remainder in Hi.
		//
		sdlRes.u.Lo = sdlTmp.u.Hi;
		sdlRes.int64 = DivMod64by32(sdlRes.int64, ulDen);
		sdlTmp.u.Hi = sdlRes.u.Hi;
		sdlRes.u.Hi = sdlRes.u.Lo;
	}

	sdlTmp.int64 = DivMod64by32(sdlTmp.int64, ulDen);
	sdlRes.u.Lo = sdlTmp.u.Lo;
	*pdlNum = sdlRes.int64;
	return sdlTmp.u.Hi;
}

Int32 SearchScale(UInt32 ulResHi, UInt32 ulResMid, UInt32 ulResLo, Int32 iScale)
{
	Int32   iCurScale;

	// Quick check to stop us from trying to scale any more.
	//
	if (ulResHi > OVFL_MAX_1_HI || iScale >= DEC_SCALE_MAX) {
		iCurScale = 0;
		goto HaveScale;
	}

	if (iScale > DEC_SCALE_MAX - 9) {
		// We can't scale by 10^9 without exceeding the max scale factor.
		// See if we can scale to the max.  If not, we'll fall Int32o
		// standard search for scale factor.
		//
		iCurScale = DEC_SCALE_MAX - iScale;
		if (ulResHi < PowerOvfl[iCurScale - 1].Hi)
			goto HaveScale;

		if (ulResHi == PowerOvfl[iCurScale - 1].Hi) {
		UpperEq:
			if (ulResMid > PowerOvfl[iCurScale - 1].Mid ||
				(ulResMid == PowerOvfl[iCurScale - 1].Mid && ulResLo > PowerOvfl[iCurScale - 1].Lo)) {
				iCurScale--;
			}
			goto HaveScale;
		}
	}
	else if (ulResHi < OVFL_MAX_9_HI || (ulResHi == OVFL_MAX_9_HI &&
		ulResMid < OVFL_MAX_9_MID) || (ulResHi == OVFL_MAX_9_HI && ulResMid == OVFL_MAX_9_MID && ulResLo <= OVFL_MAX_9_LO))
		return 9;

	// Search for a power to scale by < 9.  Do a binary search
	// on PowerOvfl[].
	//
	iCurScale = 5;
	if (ulResHi < OVFL_MAX_5_HI)
		iCurScale = 7;
	else if (ulResHi > OVFL_MAX_5_HI)
		iCurScale = 3;
	else
		goto UpperEq;

	// iCurScale is 3 or 7.
	//
	if (ulResHi < PowerOvfl[iCurScale - 1].Hi)
		iCurScale++;
	else if (ulResHi > PowerOvfl[iCurScale - 1].Hi)
		iCurScale--;
	else
		goto UpperEq;

	// iCurScale is 2, 4, 6, or 8.
	//
	// In all cases, we already found we could not use the power one larger.
	// So if we can use this power, it is the biggest, and we're done.  If
	// we can't use this power, the one below it is correct for all cases 
	// unless it's 10^1 -- we might have to go to 10^0 (no scaling).
	// 
	if (ulResHi > PowerOvfl[iCurScale - 1].Hi)
		iCurScale--;

	if (ulResHi == PowerOvfl[iCurScale - 1].Hi)
		goto UpperEq;

HaveScale:
	// iCurScale = largest power of 10 we can scale by without overflow, 
	// iCurScale < 9.  See if this is enough to make scale factor 
	// positive if it isn't already.
	// 
	if (iCurScale + iScale < 0)
		iCurScale = -1;

	return iCurScale;
}

Int32 ScaleResult(UInt32 *rgulRes, Int32 iHiRes, Int32 iScale)
{
	Int32     iNewScale;
	Int32     iCur;
	UInt32   ulPwr;
	UInt32   ulTmp;
	UInt32   ulSticky;
	SPLIT64 sdlTmp;

	// See if we need to scale the result.  The combined scale must
	// be <= DEC_SCALE_MAX and the upper 96 bits must be zero.
	// 
	// Start by figuring a lower bound on the scaling needed to make
	// the upper 96 bits zero.  iHiRes is the index Int32o rgulRes[]
	// of the highest non-zero UInt32.
	// 
	iNewScale = iHiRes * 32 - 64 - 1;
	if (iNewScale > 0) {

		// Find the MSB.
		//
		ulTmp = rgulRes[iHiRes];
		if (!(ulTmp & 0xFFFF0000)) {
			iNewScale -= 16;
			ulTmp <<= 16;
		}
		if (!(ulTmp & 0xFF000000)) {
			iNewScale -= 8;
			ulTmp <<= 8;
		}
		if (!(ulTmp & 0xF0000000)) {
			iNewScale -= 4;
			ulTmp <<= 4;
		}
		if (!(ulTmp & 0xC0000000)) {
			iNewScale -= 2;
			ulTmp <<= 2;
		}
		if (!(ulTmp & 0x80000000)) {
			iNewScale--;
			ulTmp <<= 1;
		}

		// Multiply bit position by log10(2) to figure it's power of 10.
		// We scale the log by 256.  log(2) = .30103, * 256 = 77.  Doing this 
		// with a multiply saves a 96-byte lookup table.  The power returned
		// is <= the power of the number, so we must add one power of 10
		// to make it's Int32eger part zero after dividing by 256.
		// 
		// Note: the result of this multiplication by an approximation of
		// log10(2) have been exhaustively checked to verify it gives the 
		// correct result.  (There were only 95 to check...)
		// 
		iNewScale = ((iNewScale * 77) >> 8) + 1;

		// iNewScale = min scale factor to make high 96 bits zero, 0 - 29.
		// This reduces the scale factor of the result.  If it exceeds the
		// current scale of the result, we'll overflow.
		// 
		if (iNewScale > iScale)
			return -1;
	}
	else
		iNewScale = 0;

	// Make sure we scale by enough to bring the current scale factor
	// Int32o valid range.
	//
	if (iNewScale < iScale - DEC_SCALE_MAX)
		iNewScale = iScale - DEC_SCALE_MAX;

	if (iNewScale != 0) {
		// Scale by the power of 10 given by iNewScale.  Note that this is 
		// NOT guaranteed to bring the number within 96 bits -- it could 
		// be 1 power of 10 short.
		//
		iScale -= iNewScale;
		ulSticky = 0;
		sdlTmp.u.Hi = 0; // initialize remainder

		for (;;) {

			ulSticky |= sdlTmp.u.Hi; // record remainder as sticky bit

			if (iNewScale > POWER10_MAX)
				ulPwr = ulTenToNine;
			else
				ulPwr = ulPower10[iNewScale];

			// Compute first quotient.
			// DivMod64by32 returns quotient in Lo, remainder in Hi.
			//
			sdlTmp.int64 = DivMod64by32(rgulRes[iHiRes], ulPwr);
			rgulRes[iHiRes] = sdlTmp.u.Lo;
			iCur = iHiRes - 1;

			if (iCur >= 0) {
				// If first quotient was 0, update iHiRes.
				//
				if (sdlTmp.u.Lo == 0)
					iHiRes--;

				// Compute subsequent quotients.
				//
				do {
					sdlTmp.u.Lo = rgulRes[iCur];
					sdlTmp.int64 = DivMod64by32(sdlTmp.int64, ulPwr);
					rgulRes[iCur] = sdlTmp.u.Lo;
					iCur--;
				} while (iCur >= 0);

			}

			iNewScale -= POWER10_MAX;
			if (iNewScale > 0)
				continue; // scale some more

			// If we scaled enough, iHiRes would be 2 or less.  If not,
			// divide by 10 more.
			//
			if (iHiRes > 2) {
				iNewScale = 1;
				iScale--;
				continue; // scale by 10
			}

			// Round final result.  See if remainder >= 1/2 of divisor.
			// If remainder == 1/2 divisor, round up if odd or sticky bit set.
			//
			ulPwr >>= 1;  // power of 10 always even
			if (ulPwr <= sdlTmp.u.Hi && (ulPwr < sdlTmp.u.Hi ||
				((rgulRes[0] & 1) | ulSticky))) {
				iCur = -1;
				while (++rgulRes[++iCur] == 0);

				if (iCur > 2) {
					// The rounding caused us to carry beyond 96 bits. 
					// Scale by 10 more.
					//
					iHiRes = iCur;
					ulSticky = 0;  // no sticky bit
					sdlTmp.u.Hi = 0; // or remainder
					iNewScale = 1;
					iScale--;
					continue; // scale by 10
				}
			}

			// We may have scaled it more than we planned.  Make sure the scale 
			// factor hasn't gone negative, indicating overflow.
			// 
			if (iScale < 0)
				return -1;

			return iScale;
		} // for(;;)
	}
	return iScale;
}

Int32 DecAddSub(Int32* d1, Int32* d2, Int32* res, Byte bSign)
{
	DECIMAL* pdecL = (DECIMAL*)d1;
	DECIMAL* pdecR = (DECIMAL*)d2;
	DECIMAL* pdecRes = (DECIMAL*)res;

	UInt32     rgulNum[6];
	UInt32     ulPwr;
	Int32       iScale;
	Int32       iHiProd;
	Int32       iCur;
	SPLIT64   sdlTmp;
	DECIMAL   decRes;
	DECIMAL   decTmp;
	DECIMAL* pdecTmp;

	bSign ^= (DECIMAL_SIGN(*pdecR) ^ DECIMAL_SIGN(*pdecL)) & DECIMAL_NEG;

	if (DECIMAL_SCALE(*pdecR) == DECIMAL_SCALE(*pdecL)) {
		// Scale factors are equal, no alignment necessary.
		//
		DECIMAL_SIGNSCALE(decRes) = DECIMAL_SIGNSCALE(*pdecL);

	AlignedAdd:
		if (bSign) {
			// Signs differ - subtract
			//
			DECIMAL_LO64_SET(decRes, (DECIMAL_LO64_GET(*pdecL) - DECIMAL_LO64_GET(*pdecR)));
			DECIMAL_HI32(decRes) = DECIMAL_HI32(*pdecL) - DECIMAL_HI32(*pdecR);

			// Propagate carry
			//
			if (DECIMAL_LO64_GET(decRes) > DECIMAL_LO64_GET(*pdecL)) {
				DECIMAL_HI32(decRes)--;
				if (DECIMAL_HI32(decRes) >= DECIMAL_HI32(*pdecL))
					goto SignFlip;
			}
			else if (DECIMAL_HI32(decRes) > DECIMAL_HI32(*pdecL)) {
				// Got negative result.  Flip its sign.
				// 
			SignFlip:
				DECIMAL_LO64_SET(decRes, -(Int64)DECIMAL_LO64_GET(decRes));
				DECIMAL_HI32(decRes) = ~DECIMAL_HI32(decRes);
				if (DECIMAL_LO64_GET(decRes) == 0)
					DECIMAL_HI32(decRes)++;
				DECIMAL_SIGN(decRes) ^= DECIMAL_NEG;
			}

		}
		else {
			// Signs are the same - add
			//
			DECIMAL_LO64_SET(decRes, (DECIMAL_LO64_GET(*pdecL) + DECIMAL_LO64_GET(*pdecR)));
			DECIMAL_HI32(decRes) = DECIMAL_HI32(*pdecL) + DECIMAL_HI32(*pdecR);

			// Propagate carry
			//
			if (DECIMAL_LO64_GET(decRes) < DECIMAL_LO64_GET(*pdecL)) {
				DECIMAL_HI32(decRes)++;
				if (DECIMAL_HI32(decRes) <= DECIMAL_HI32(*pdecL))
					goto AlignedScale;
			}
			else if (DECIMAL_HI32(decRes) < DECIMAL_HI32(*pdecL)) {
			AlignedScale:
				// The addition carried above 96 bits.  Divide the result by 10,
				// dropping the scale factor.
				// 
				if (DECIMAL_SCALE(decRes) == 0)
					return DISP_E_OVERFLOW;
				DECIMAL_SCALE(decRes)--;

				sdlTmp.u.Lo = DECIMAL_HI32(decRes);
				sdlTmp.u.Hi = 1;
				sdlTmp.int64 = DivMod64by32(sdlTmp.int64, 10);
				DECIMAL_HI32(decRes) = sdlTmp.u.Lo;

				sdlTmp.u.Lo = DECIMAL_MID32(decRes);
				sdlTmp.int64 = DivMod64by32(sdlTmp.int64, 10);
				DECIMAL_MID32(decRes) = sdlTmp.u.Lo;

				sdlTmp.u.Lo = DECIMAL_LO32(decRes);
				sdlTmp.int64 = DivMod64by32(sdlTmp.int64, 10);
				DECIMAL_LO32(decRes) = sdlTmp.u.Lo;

				// See if we need to round up.
				//
				if (sdlTmp.u.Hi >= 5 && (sdlTmp.u.Hi > 5 || (DECIMAL_LO32(decRes) & 1))) {
					DECIMAL_LO64_SET(decRes, DECIMAL_LO64_GET(decRes) + 1);
					if (DECIMAL_LO64_GET(decRes) == 0)
						DECIMAL_HI32(decRes)++;
				}
			}
		}
	}
	else {
		// Scale factors are not equal.  Assume that a larger scale
		// factor (more decimal places) is likely to mean that number
		// is smaller.  Start by guessing that the right operand has
		// the larger scale factor.  The result will have the larger
		// scale factor.
		//
		DECIMAL_SCALE(decRes) = DECIMAL_SCALE(*pdecR);  // scale factor of "smaller"
		DECIMAL_SIGN(decRes) = DECIMAL_SIGN(*pdecL);    // but sign of "larger"
		iScale = DECIMAL_SCALE(decRes) - DECIMAL_SCALE(*pdecL);

		if (iScale < 0) {
			iScale = -iScale;
			DECIMAL_SCALE(decRes) = DECIMAL_SCALE(*pdecL);
			DECIMAL_SIGN(decRes) ^= bSign;
			pdecTmp = pdecR;
			pdecR = pdecL;
			pdecL = pdecTmp;
		}

		// *pdecL will need to be multiplied by 10^iScale so
		// it will have the same scale as *pdecR.  We could be
		// extending it to up to 192 bits of precision.
		//
		if (iScale <= POWER10_MAX) {
			// Scaling won't make it larger than 4 UInt32s
			//
			ulPwr = ulPower10[iScale];
			DECIMAL_LO64_SET(decTmp, UInt32x32To64(DECIMAL_LO32(*pdecL), ulPwr));
			sdlTmp.int64 = UInt32x32To64(DECIMAL_MID32(*pdecL), ulPwr);
			sdlTmp.int64 += DECIMAL_MID32(decTmp);
			DECIMAL_MID32(decTmp) = sdlTmp.u.Lo;
			DECIMAL_HI32(decTmp) = sdlTmp.u.Hi;
			sdlTmp.int64 = UInt32x32To64(DECIMAL_HI32(*pdecL), ulPwr);
			sdlTmp.int64 += DECIMAL_HI32(decTmp);
			if (sdlTmp.u.Hi == 0) {
				// Result fits in 96 bits.  Use standard aligned add.
				//
				DECIMAL_HI32(decTmp) = sdlTmp.u.Lo;
				pdecL = &decTmp;
				goto AlignedAdd;
			}
			rgulNum[0] = DECIMAL_LO32(decTmp);
			rgulNum[1] = DECIMAL_MID32(decTmp);
			rgulNum[2] = sdlTmp.u.Lo;
			rgulNum[3] = sdlTmp.u.Hi;
			iHiProd = 3;
		}
		else {
			// Have to scale by a bunch.  Move the number to a buffer
			// where it has room to grow as it's scaled.
			//
			rgulNum[0] = DECIMAL_LO32(*pdecL);
			rgulNum[1] = DECIMAL_MID32(*pdecL);
			rgulNum[2] = DECIMAL_HI32(*pdecL);
			iHiProd = 2;

			// Scan for zeros in the upper words.
			//
			if (rgulNum[2] == 0) {
				iHiProd = 1;
				if (rgulNum[1] == 0) {
					iHiProd = 0;
					if (rgulNum[0] == 0) {
						// Left arg is zero, return right.
						//
						DECIMAL_LO64_SET(decRes, DECIMAL_LO64_GET(*pdecR));
						DECIMAL_HI32(decRes) = DECIMAL_HI32(*pdecR);
						DECIMAL_SIGN(decRes) ^= bSign;
						goto RetDec;
					}
				}
			}

			// Scaling loop, up to 10^9 at a time.  iHiProd stays updated
			// with index of highest non-zero UInt32.
			//
			for (; iScale > 0; iScale -= POWER10_MAX) {
				if (iScale > POWER10_MAX)
					ulPwr = ulTenToNine;
				else
					ulPwr = ulPower10[iScale];

				sdlTmp.u.Hi = 0;
				for (iCur = 0; iCur <= iHiProd; iCur++) {
					sdlTmp.int64 = UInt32x32To64(rgulNum[iCur], ulPwr) + sdlTmp.u.Hi;
					rgulNum[iCur] = sdlTmp.u.Lo;
				}

				if (sdlTmp.u.Hi != 0)
					// We're extending the result by another UInt32.
					rgulNum[++iHiProd] = sdlTmp.u.Hi;
			}
		}

		// Scaling complete, do the add.  Could be subtract if signs differ.
		//
		sdlTmp.u.Lo = rgulNum[0];
		sdlTmp.u.Hi = rgulNum[1];

		if (bSign) {
			// Signs differ, subtract.
			//
			DECIMAL_LO64_SET(decRes, (sdlTmp.int64 - DECIMAL_LO64_GET(*pdecR)));
			DECIMAL_HI32(decRes) = rgulNum[2] - DECIMAL_HI32(*pdecR);

			// Propagate carry
			//
			if (DECIMAL_LO64_GET(decRes) > sdlTmp.int64) {
				DECIMAL_HI32(decRes)--;
				if (DECIMAL_HI32(decRes) >= rgulNum[2])
					goto LongSub;
			}
			else if (DECIMAL_HI32(decRes) > rgulNum[2]) {
			LongSub:
				if (iHiProd <= 2)
					goto SignFlip;

				iCur = 3;
				while (rgulNum[iCur++]-- == 0);
				if (rgulNum[iHiProd] == 0)
					iHiProd--;
			}
		}
		else {
			// Signs the same, add.
			//
			DECIMAL_LO64_SET(decRes, (sdlTmp.int64 + DECIMAL_LO64_GET(*pdecR)));
			DECIMAL_HI32(decRes) = rgulNum[2] + DECIMAL_HI32(*pdecR);

			// Propagate carry
			//
			if (DECIMAL_LO64_GET(decRes) < sdlTmp.int64) {
				DECIMAL_HI32(decRes)++;
				if (DECIMAL_HI32(decRes) <= rgulNum[2])
					goto LongAdd;
			}
			else if (DECIMAL_HI32(decRes) < rgulNum[2]) {
			LongAdd:
				// Had a carry above 96 bits.
				//
				iCur = 3;
				do {
					if (iHiProd < iCur) {
						rgulNum[iCur] = 1;
						iHiProd = iCur;
						break;
					}
				} while (++rgulNum[iCur++] == 0);
			}
		}

		if (iHiProd > 2) {
			rgulNum[0] = DECIMAL_LO32(decRes);
			rgulNum[1] = DECIMAL_MID32(decRes);
			rgulNum[2] = DECIMAL_HI32(decRes);
			DECIMAL_SCALE(decRes) = (Byte)ScaleResult(rgulNum, iHiProd, DECIMAL_SCALE(decRes));
			if (DECIMAL_SCALE(decRes) == (Byte)-1)
				return DISP_E_OVERFLOW;

			DECIMAL_LO32(decRes) = rgulNum[0];
			DECIMAL_MID32(decRes) = rgulNum[1];
			DECIMAL_HI32(decRes) = rgulNum[2];
		}
	}

RetDec:
	COPYDEC(*pdecRes, decRes)
		return NOERROR;
}

#define VARCMP_LT   -1
#define VARCMP_EQ   0
#define VARCMP_GT   1

Int32 DecCmp(Int32* d1, Int32* d2)
{
	DECIMAL* pdecL = (DECIMAL*)d1;
	DECIMAL* pdecR = (DECIMAL*)d2;

	UInt32   ulSgnL;
	UInt32   ulSgnR;

	// First check signs and whether either are zero.  If both are
	// non-zero and of the same sign, just use subtraction to compare.
	//
	ulSgnL = DECIMAL_LO32(*pdecL) | DECIMAL_MID32(*pdecL) | DECIMAL_HI32(*pdecL);
	ulSgnR = DECIMAL_LO32(*pdecR) | DECIMAL_MID32(*pdecR) | DECIMAL_HI32(*pdecR);
	if (ulSgnL != 0)
		ulSgnL = (DECIMAL_SIGN(*pdecL) & DECIMAL_NEG) | 1;

	if (ulSgnR != 0)
		ulSgnR = (DECIMAL_SIGN(*pdecR) & DECIMAL_NEG) | 1;

	// ulSgnL & ulSgnR have values 1, 0, or 0x81 depending on if the left/right
	// operand is +, 0, or -.
	//
	if (ulSgnL == ulSgnR) {
		if (ulSgnL == 0)    // both are zero
			return VARCMP_EQ; // return equal

		DECIMAL decRes;

		DecAddSub((Int32*)pdecL, (Int32*)pdecR, (Int32*)&decRes, DECIMAL_NEG);
		if (DECIMAL_LO64_GET(decRes) == 0 && DECIMAL_HI32(decRes) == 0)
			return VARCMP_EQ;
		if (DECIMAL_SIGN(decRes) & DECIMAL_NEG)
			return VARCMP_LT;
		return VARCMP_GT;
	}

	// Signs are different.  Used signed byte compares
	//
	if ((Byte)ulSgnL > (Byte)ulSgnR)
		return VARCMP_GT;
	return VARCMP_LT;
}

Int32 DecFromR4(Single fltIn, Int32* pdec)
{
	DECIMAL* pdecOut = (DECIMAL*)pdec;

	Int32         iExp;    // number of bits to left of binary poInt32
	Int32         iPower;
	UInt32		  ulMant;
	Double        dbl;
	SPLIT64       sdlLo;
	SPLIT64       sdlHi;
	Int32         lmax, cur;  // temps used during scale reduction

	// The most we can scale by is 10^28, which is just slightly more
	// than 2^93.  So a Single with an exponent of -94 could just
	// barely reach 0.5, but smaller exponents will always round to zero.
	//
	if ((iExp = ((SNGSTRUCT *)&fltIn)->exp - SNGBIAS) < -94)
	{
		DECIMAL_SETZERO(*pdecOut);
		return NOERROR;
	}

	if (iExp > 96)
		return DISP_E_OVERFLOW;

	// Round the input to a 7-digit Int32eger.  The R4 format has
	// only 7 digits of precision, and we want to keep garbage digits
	// out of the Decimal were making.
	//
	// Calculate max power of 10 input value could have by multiplying 
	// the exponent by log10(2).  Using scaled Int32eger multiplcation, 
	// log10(2) * 2 ^ 16 = .30103 * 65536 = 19728.3.
	//
	dbl = fabs(fltIn);
	iPower = 6 - ((iExp * 19728) >> 16);

	if (iPower >= 0) {
		// We have less than 7 digits, scale input up.
		//
		if (iPower > DECMAX)
			iPower = DECMAX;

		dbl = dbl * dblPower10[iPower];
	}
	else {
		if (iPower != -1 || dbl >= 1E7)
			dbl = dbl / fnDblPower10(-iPower);
		else
			iPower = 0; // didn't scale it
	}

	if (dbl < 1E6 && iPower < DECMAX)
	{
		dbl *= 10;
		iPower++;
	}

	// Round to Int32eger
	//
	ulMant = (Int32)dbl;
	dbl -= (Double)ulMant;  // difference between input & Int32eger
	if (dbl > 0.5 || dbl == 0.5 && (ulMant & 1))
		ulMant++;

	if (ulMant == 0)
	{
		DECIMAL_SETZERO(*pdecOut);
		return NOERROR;
	}

	if (iPower < 0) {
		// Add -iPower factors of 10, -iPower <= (29 - 7) = 22.
		//
		iPower = -iPower;
		if (iPower < 10) {
			sdlLo.int64 = UInt32x32To64(ulMant, (UInt32)ulPower10[iPower]);

			DECIMAL_LO32(*pdecOut) = sdlLo.u.Lo;
			DECIMAL_MID32(*pdecOut) = sdlLo.u.Hi;
			DECIMAL_HI32(*pdecOut) = 0;
		}
		else {
			// Have a big power of 10.
			//
			if (iPower > 18) {
				sdlLo.int64 = UInt32x32To64(ulMant, (UInt32)ulPower10[iPower - 18]);
				sdlLo.int64 = UInt64x64To128(sdlLo, sdlTenToEighteen, &sdlHi.int64);

				if (sdlHi.u.Hi != 0)
					return DISP_E_OVERFLOW;
			}
			else {
				sdlLo.int64 = UInt32x32To64(ulMant, (UInt32)ulPower10[iPower - 9]);
				sdlHi.int64 = UInt32x32To64(ulTenToNine, sdlLo.u.Hi);
				sdlLo.int64 = UInt32x32To64(ulTenToNine, sdlLo.u.Lo);
				sdlHi.int64 += sdlLo.u.Hi;
				sdlLo.u.Hi = sdlHi.u.Lo;
				sdlHi.u.Lo = sdlHi.u.Hi;
			}
			DECIMAL_LO32(*pdecOut) = sdlLo.u.Lo;
			DECIMAL_MID32(*pdecOut) = sdlLo.u.Hi;
			DECIMAL_HI32(*pdecOut) = sdlHi.u.Lo;
		}
		DECIMAL_SCALE(*pdecOut) = 0;
	}
	else {
		// Factor out powers of 10 to reduce the scale, if possible.
		// The maximum number we could factor out would be 6.  This
		// comes from the fact we have a 7-digit number, and the
		// MSD must be non-zero -- but the lower 6 digits could be
		// zero.  Note also the scale factor is never negative, so
		// we can't scale by any more than the power we used to
		// get the Int32eger.
		//
		// DivMod32by32 returns the quotient in Lo, the remainder in Hi.
		//
		lmax = min(iPower, 6);

		// lmax is the largest power of 10 to try, lmax <= 6.
		// We'll try powers 4, 2, and 1 unless they're too big.
		//
		for (cur = 4; cur > 0; cur >>= 1)
		{
			if (cur > lmax)
				continue;

			sdlLo.int64 = DivMod32by32(ulMant, (UInt32)ulPower10[cur]);

			if (sdlLo.u.Hi == 0) {
				ulMant = sdlLo.u.Lo;
				iPower -= cur;
				lmax -= cur;
			}
		}
		DECIMAL_LO32(*pdecOut) = ulMant;
		DECIMAL_MID32(*pdecOut) = 0;
		DECIMAL_HI32(*pdecOut) = 0;
		DECIMAL_SCALE(*pdecOut) = iPower;
	}

	DECIMAL_SIGN(*pdecOut) = (Byte)((SNGSTRUCT *)&fltIn)->sign << 7;
	return NOERROR;
}

Int32 DecFromR8(Double dblIn, Int32* pdec)
{
	DECIMAL* pdecOut = (DECIMAL*)pdec;

	Int32         iExp;    // number of bits to left of binary poInt32
	Int32         iPower;  // power-of-10 scale factor
	SPLIT64       sdlMant;
	SPLIT64       sdlLo;
	Double        dbl;
	Int32         lmax, cur;  // temps used during scale reduction
	UInt32         ulPwrCur;
	UInt32         ulQuo;


	// The most we can scale by is 10^28, which is just slightly more
	// than 2^93.  So a Single with an exponent of -94 could just
	// barely reach 0.5, but smaller exponents will always round to zero.
	//
	if ((iExp = ((DBLSTRUCT *)&dblIn)->u.exp - DBLBIAS) < -94)
	{
		DECIMAL_SETZERO(*pdecOut);
		return NOERROR;
	}

	if (iExp > 96)
		return DISP_E_OVERFLOW;

	// Round the input to a 15-digit Int32eger.  The R8 format has
	// only 15 digits of precision, and we want to keep garbage digits
	// out of the Decimal were making.
	//
	// Calculate max power of 10 input value could have by multiplying 
	// the exponent by log10(2).  Using scaled Int32eger multiplcation, 
	// log10(2) * 2 ^ 16 = .30103 * 65536 = 19728.3.
	//
	dbl = fabs(dblIn);
	iPower = 14 - ((iExp * 19728) >> 16);

	if (iPower >= 0) {
		// We have less than 15 digits, scale input up.
		//
		if (iPower > DECMAX)
			iPower = DECMAX;

		dbl = dbl * dblPower10[iPower];
	}
	else {
		if (iPower != -1 || dbl >= 1E15)
			dbl = dbl / fnDblPower10(-iPower);
		else
			iPower = 0; // didn't scale it
	}

	if (dbl < 1E14 && iPower < DECMAX)
	{
		dbl *= 10;
		iPower++;
	}

	// Round to int64
	//
	sdlMant.int64 = (Int64)dbl;
	dbl -= (Double)(Int64)sdlMant.int64;  // dif between input & Int32eger
	if (dbl > 0.5 || dbl == 0.5 && (sdlMant.u.Lo & 1))
		sdlMant.int64++;

	if (sdlMant.int64 == 0)
	{
		DECIMAL_SETZERO(*pdecOut);
		return NOERROR;
	}

	if (iPower < 0) {
		// Add -iPower factors of 10, -iPower <= (29 - 15) = 14.
		//
		iPower = -iPower;
		if (iPower < 10) {
			sdlLo.int64 = UInt32x32To64(sdlMant.u.Lo, (UInt32)ulPower10[iPower]);
			sdlMant.int64 = UInt32x32To64(sdlMant.u.Hi, (UInt32)ulPower10[iPower]);
			sdlMant.int64 += sdlLo.u.Hi;
			sdlLo.u.Hi = sdlMant.u.Lo;
			sdlMant.u.Lo = sdlMant.u.Hi;
		}
		else {
			// Have a big power of 10.
			//
			sdlLo.int64 = UInt64x64To128(sdlMant, sdlPower10[iPower - 10], &sdlMant.int64);

			if (sdlMant.u.Hi != 0)
				return DISP_E_OVERFLOW;
		}
		DECIMAL_LO32(*pdecOut) = sdlLo.u.Lo;
		DECIMAL_MID32(*pdecOut) = sdlLo.u.Hi;
		DECIMAL_HI32(*pdecOut) = sdlMant.u.Lo;
		DECIMAL_SCALE(*pdecOut) = 0;
	}
	else {
		// Factor out powers of 10 to reduce the scale, if possible.
		// The maximum number we could factor out would be 14.  This
		// comes from the fact we have a 15-digit number, and the 
		// MSD must be non-zero -- but the lower 14 digits could be 
		// zero.  Note also the scale factor is never negative, so
		// we can't scale by any more than the power we used to
		// get the Int32eger.
		//
		// DivMod64by32 returns the quotient in Lo, the remainder in Hi.
		//
		lmax = min(iPower, 14);

		// lmax is the largest power of 10 to try, lmax <= 14.
		// We'll try powers 8, 4, 2, and 1 unless they're too big.
		//
		for (cur = 8; cur > 0; cur >>= 1)
		{
			if (cur > lmax)
				continue;

			ulPwrCur = (UInt32)ulPower10[cur];

			if (sdlMant.u.Hi >= ulPwrCur) {
				// Overflow if we try to divide in one step.
				//
				sdlLo.int64 = DivMod64by32(sdlMant.u.Hi, ulPwrCur);
				ulQuo = sdlLo.u.Lo;
				sdlLo.u.Lo = sdlMant.u.Lo;
				sdlLo.int64 = DivMod64by32(sdlLo.int64, ulPwrCur);
			}
			else {
				ulQuo = 0;
				sdlLo.int64 = DivMod64by32(sdlMant.int64, ulPwrCur);
			}

			if (sdlLo.u.Hi == 0) {
				sdlMant.u.Hi = ulQuo;
				sdlMant.u.Lo = sdlLo.u.Lo;
				iPower -= cur;
				lmax -= cur;
			}
		}

		DECIMAL_HI32(*pdecOut) = 0;
		DECIMAL_SCALE(*pdecOut) = iPower;
		DECIMAL_LO32(*pdecOut) = sdlMant.u.Lo;
		DECIMAL_MID32(*pdecOut) = sdlMant.u.Hi;
	}

	DECIMAL_SIGN(*pdecOut) = (Byte)((DBLSTRUCT *)&dblIn)->u.sign << 7;
	return NOERROR;
}

Int32 DecMul(Int32* d1, Int32* d2, Int32* res)
{
	DECIMAL* pdecL = (DECIMAL*)d1;
	DECIMAL* pdecR = (DECIMAL*)d2;
	DECIMAL* pdecRes = (DECIMAL*)res;

	SPLIT64 sdlTmp;
	SPLIT64 sdlTmp2;
	SPLIT64 sdlTmp3;
	Int32   iScale;
	Int32   iHiProd;
	UInt32  ulPwr;
	UInt32  ulRemLo;
	UInt32  ulRemHi;
	UInt32  rgulProd[6];

	iScale = pdecL->u.u.scale + pdecR->u.u.scale;

	if ((pdecL->Hi32 | pdecL->v.v.Mid32 | pdecR->Hi32 | pdecR->v.v.Mid32) == 0)
	{
		// Upper 64 bits are zero.
		//
		sdlTmp.int64 = UInt32x32To64(pdecL->v.v.Lo32, pdecR->v.v.Lo32);
		if (iScale > DEC_SCALE_MAX)
		{
			// Result iScale is too big.  Divide result by power of 10 to reduce it.
			// If the amount to divide by is > 19 the result is guaranteed
			// less than 1/2.  [max value in 64 bits = 1.84E19]
			//
			iScale -= DEC_SCALE_MAX;
			if (iScale > 19)
			{
			ReturnZero:
				DECIMAL_SETZERO(*pdecRes);
				return NOERROR;
			}
			if (iScale > POWER10_MAX)
			{
				// Divide by 1E10 first, to get the power down to a 32-bit quantity.
				// 1E10 itself doesn't fit in 32 bits, so we'll divide by 2.5E9 now
				// then multiply the next divisor by 4 (which will be a max of 4E9).
				// 
				ulRemLo = FullDiv64By32(&sdlTmp.int64, ulTenToTenDiv4);
				ulPwr = ulPower10[iScale - 10] << 2;
			}
			else
			{
				ulPwr = ulPower10[iScale];
				ulRemLo = 0;
			}

			// Power to divide by fits in 32 bits.
			//
			ulRemHi = FullDiv64By32(&sdlTmp.int64, ulPwr);

			// Round result.  See if remainder >= 1/2 of divisor.
			// Divisor is a power of 10, so it is always even.
			//
			ulPwr >>= 1;
			if (ulRemHi >= ulPwr && (ulRemHi > ulPwr || (ulRemLo | (sdlTmp.u.Lo & 1))))
				sdlTmp.int64++;

			iScale = DEC_SCALE_MAX;
		}
		DECIMAL_LO32(*pdecRes) = sdlTmp.u.Lo;
		DECIMAL_MID32(*pdecRes) = sdlTmp.u.Hi;
		DECIMAL_HI32(*pdecRes) = 0;
	}
	else
	{

		// At least one operand has bits set in the upper 64 bits.
		//
		// Compute and accumulate the 9 partial products into a 
		// 192-bit (24-byte) result.
		//
		//                [l-h][l-m][l-l]   left high, middle, low
		//             x  [r-h][r-m][r-l]   right high, middle, low
		// ------------------------------
		//
		//                     [0-h][0-l]   l-l * r-l
		//                [1ah][1al]        l-l * r-m
		//                [1bh][1bl]        l-m * r-l
		//           [2ah][2al]             l-m * r-m
		//           [2bh][2bl]             l-l * r-h
		//           [2ch][2cl]             l-h * r-l
		//      [3ah][3al]                  l-m * r-h
		//      [3bh][3bl]                  l-h * r-m
		// [4-h][4-l]                       l-h * r-h
		// ------------------------------
		// [p-5][p-4][p-3][p-2][p-1][p-0]   prod[] array
		//
		sdlTmp.int64 = UInt32x32To64(pdecL->v.v.Lo32, pdecR->v.v.Lo32);
		rgulProd[0] = sdlTmp.u.Lo;

		sdlTmp2.int64 = UInt32x32To64(pdecL->v.v.Lo32, pdecR->v.v.Mid32) + sdlTmp.u.Hi;

		sdlTmp.int64 = UInt32x32To64(pdecL->v.v.Mid32, pdecR->v.v.Lo32);
		sdlTmp.int64 += sdlTmp2.int64; // this could generate carry
		rgulProd[1] = sdlTmp.u.Lo;
		if (sdlTmp.int64 < sdlTmp2.int64) // detect carry
			sdlTmp2.u.Hi = 1;
		else
			sdlTmp2.u.Hi = 0;
		sdlTmp2.u.Lo = sdlTmp.u.Hi;

		sdlTmp.int64 = UInt32x32To64(pdecL->v.v.Mid32, pdecR->v.v.Mid32) + sdlTmp2.int64;

		if (pdecL->Hi32 | pdecR->Hi32) {
			// Highest 32 bits is non-zero.  Calculate 5 more partial products.
			//
			sdlTmp2.int64 = UInt32x32To64(pdecL->v.v.Lo32, pdecR->Hi32);
			sdlTmp.int64 += sdlTmp2.int64; // this could generate carry
			if (sdlTmp.int64 < sdlTmp2.int64) // detect carry
				sdlTmp3.u.Hi = 1;
			else
				sdlTmp3.u.Hi = 0;

			sdlTmp2.int64 = UInt32x32To64(pdecL->Hi32, pdecR->v.v.Lo32);
			sdlTmp.int64 += sdlTmp2.int64; // this could generate carry
			rgulProd[2] = sdlTmp.u.Lo;
			if (sdlTmp.int64 < sdlTmp2.int64) // detect carry
				sdlTmp3.u.Hi++;
			sdlTmp3.u.Lo = sdlTmp.u.Hi;

			sdlTmp.int64 = UInt32x32To64(pdecL->v.v.Mid32, pdecR->Hi32);
			sdlTmp.int64 += sdlTmp3.int64; // this could generate carry
			if (sdlTmp.int64 < sdlTmp3.int64) // detect carry
				sdlTmp3.u.Hi = 1;
			else
				sdlTmp3.u.Hi = 0;

			sdlTmp2.int64 = UInt32x32To64(pdecL->Hi32, pdecR->v.v.Mid32);
			sdlTmp.int64 += sdlTmp2.int64; // this could generate carry
			rgulProd[3] = sdlTmp.u.Lo;
			if (sdlTmp.int64 < sdlTmp2.int64) // detect carry
				sdlTmp3.u.Hi++;
			sdlTmp3.u.Lo = sdlTmp.u.Hi;

			sdlTmp.int64 = UInt32x32To64(pdecL->Hi32, pdecR->Hi32) + sdlTmp3.int64;
			rgulProd[4] = sdlTmp.u.Lo;
			rgulProd[5] = sdlTmp.u.Hi;

			iHiProd = 5;
		}
		else {
			rgulProd[2] = sdlTmp.u.Lo;
			rgulProd[3] = sdlTmp.u.Hi;
			iHiProd = 3;
		}

		// Check for leading zero UInt32s on the product
		//
		while (rgulProd[iHiProd] == 0) {
			iHiProd--;
			if (iHiProd < 0)
				goto ReturnZero;
		}

		iScale = ScaleResult(rgulProd, iHiProd, iScale);
		if (iScale == -1)
			return DISP_E_OVERFLOW;

		pdecRes->v.v.Lo32 = rgulProd[0];
		pdecRes->v.v.Mid32 = rgulProd[1];
		pdecRes->Hi32 = rgulProd[2];
	}

	pdecRes->u.u.sign = pdecR->u.u.sign ^ pdecL->u.u.sign;
	pdecRes->u.u.scale = (Byte)iScale;
	return NOERROR;
}

// Add a 32 bit unsigned long to an array of 3 unsigned longs representing a 96 integer
// Returns 0 if there is an overflow
Byte Add32To96(UInt32* rgulNum, UInt32 ulValue) {
	rgulNum[0] += ulValue;
	if (rgulNum[0] < ulValue) {
		if (++rgulNum[1] == 0) {
			if (++rgulNum[2] == 0) {
				return 0;
			}
		}
	}
	return 1;
}

UInt32 IncreaseScale(UInt32 *rgulNum, UInt32 ulPwr)
{
	SPLIT64   sdlTmp;

	sdlTmp.int64 = UInt32x32To64(rgulNum[0], ulPwr);
	rgulNum[0] = sdlTmp.u.Lo;
	sdlTmp.int64 = UInt32x32To64(rgulNum[1], ulPwr) + sdlTmp.u.Hi;
	rgulNum[1] = sdlTmp.u.Lo;
	sdlTmp.int64 = UInt32x32To64(rgulNum[2], ulPwr) + sdlTmp.u.Hi;
	rgulNum[2] = sdlTmp.u.Lo;
	return sdlTmp.u.Hi;
}

// Adjust the quotient to deal with an overflow. We need to divide by 10, 
// feed in the high bit to undo the overflow and then round as required, 
Void OverflowUnscale(UInt32* rgulQuo, Byte fRemainder) {
	SPLIT64  sdlTmp;

	// We have overflown, so load the high bit with a one.
	sdlTmp.u.Hi = 1u;
	sdlTmp.u.Lo = rgulQuo[2];
	sdlTmp.int64 = DivMod64by32(sdlTmp.int64, 10u);
	rgulQuo[2] = sdlTmp.u.Lo;
	sdlTmp.u.Lo = rgulQuo[1];
	sdlTmp.int64 = DivMod64by32(sdlTmp.int64, 10u);
	rgulQuo[1] = sdlTmp.u.Lo;
	sdlTmp.u.Lo = rgulQuo[0];
	sdlTmp.int64 = DivMod64by32(sdlTmp.int64, 10u);
	rgulQuo[0] = sdlTmp.u.Lo;
	// The remainder is the last digit that does not fit, so we can use it to work out if we need to round up
	if ((sdlTmp.u.Hi > 5) || ((sdlTmp.u.Hi == 5) && (fRemainder || (rgulQuo[0] & 1)))) {
		Add32To96(rgulQuo, 1u);
	}
}

UInt32 Div96By32(UInt32 *rgulNum, UInt32 ulDen)
{
	SPLIT64  sdlTmp;

	sdlTmp.u.Hi = 0;

	if (rgulNum[2] != 0)
		goto Div3Word;

	if (rgulNum[1] >= ulDen)
		goto Div2Word;

	sdlTmp.u.Hi = rgulNum[1];
	rgulNum[1] = 0;
	goto Div1Word;

Div3Word:
	sdlTmp.u.Lo = rgulNum[2];
	sdlTmp.int64 = DivMod64by32(sdlTmp.int64, ulDen);
	rgulNum[2] = sdlTmp.u.Lo;
Div2Word:
	sdlTmp.u.Lo = rgulNum[1];
	sdlTmp.int64 = DivMod64by32(sdlTmp.int64, ulDen);
	rgulNum[1] = sdlTmp.u.Lo;
Div1Word:
	sdlTmp.u.Lo = rgulNum[0];
	sdlTmp.int64 = DivMod64by32(sdlTmp.int64, ulDen);
	rgulNum[0] = sdlTmp.u.Lo;
	return sdlTmp.u.Hi;
}

UInt32 Div96By64(UInt32* rgulNum, SPLIT64 sdlDen)
{
	SPLIT64 sdlQuo;
	SPLIT64 sdlNum;
	SPLIT64 sdlProd;

	sdlNum.u.Lo = rgulNum[0];

	if (rgulNum[2] >= sdlDen.u.Hi) {
		// Divide would overflow.  Assume a quotient of 2^32, and set
		// up remainder accordingly.  Then jump to loop which reduces
		// the quotient.
		//
		sdlNum.u.Hi = rgulNum[1] - sdlDen.u.Lo;
		sdlQuo.u.Lo = 0;
		goto NegRem;
	}

	// Hardware divide won't overflow
	//
	if (rgulNum[2] == 0 && rgulNum[1] < sdlDen.u.Hi)
		// Result is zero.  Entire dividend is remainder.
		//
		return 0;

	// DivMod64by32 returns quotient in Lo, remainder in Hi.
	//
	sdlQuo.u.Lo = rgulNum[1];
	sdlQuo.u.Hi = rgulNum[2];
	sdlQuo.int64 = DivMod64by32(sdlQuo.int64, sdlDen.u.Hi);
	sdlNum.u.Hi = sdlQuo.u.Hi; // remainder

	// Compute full remainder, rem = dividend - (quo * divisor).
	//
	sdlProd.int64 = UInt32x32To64(sdlQuo.u.Lo, sdlDen.u.Lo); // quo * lo divisor
	sdlNum.int64 -= sdlProd.int64;

	if (sdlNum.int64 > ~sdlProd.int64) {
	NegRem:
		// Remainder went negative.  Add divisor back in until it's positive,
		// a max of 2 times.
		//
		do {
			sdlQuo.u.Lo--;
			sdlNum.int64 += sdlDen.int64;
		} while (sdlNum.int64 >= sdlDen.int64);
	}

	rgulNum[0] = sdlNum.u.Lo;
	rgulNum[1] = sdlNum.u.Hi;
	return sdlQuo.u.Lo;
}

UInt32 Div128By96(UInt32 *rgulNum, UInt32 *rgulDen)
{
	SPLIT64 sdlQuo;
	SPLIT64 sdlNum;
	SPLIT64 sdlProd1;
	SPLIT64 sdlProd2;

	sdlNum.u.Lo = rgulNum[0];
	sdlNum.u.Hi = rgulNum[1];

	if (rgulNum[3] == 0 && rgulNum[2] < rgulDen[2])
		// Result is zero.  Entire dividend is remainder.
		//
		return 0;

	// DivMod64by32 returns quotient in Lo, remainder in Hi.
	//
	sdlQuo.u.Lo = rgulNum[2];
	sdlQuo.u.Hi = rgulNum[3];
	sdlQuo.int64 = DivMod64by32(sdlQuo.int64, rgulDen[2]);

	// Compute full remainder, rem = dividend - (quo * divisor).
	//
	sdlProd1.int64 = UInt32x32To64(sdlQuo.u.Lo, rgulDen[0]); // quo * lo divisor
	sdlProd2.int64 = UInt32x32To64(sdlQuo.u.Lo, rgulDen[1]); // quo * mid divisor
	sdlProd2.int64 += sdlProd1.u.Hi;
	sdlProd1.u.Hi = sdlProd2.u.Lo;

	sdlNum.int64 -= sdlProd1.int64;
	rgulNum[2] = sdlQuo.u.Hi - sdlProd2.u.Hi; // sdlQuo.Hi is remainder

	// Propagate carries
	//
	if (sdlNum.int64 > ~sdlProd1.int64) {
		rgulNum[2]--;
		if (rgulNum[2] >= ~sdlProd2.u.Hi)
			goto NegRem;
	}
	else if (rgulNum[2] > ~sdlProd2.u.Hi) {
	NegRem:
		// Remainder went negative.  Add divisor back in until it's positive,
		// a max of 2 times.
		//
		sdlProd1.u.Lo = rgulDen[0];
		sdlProd1.u.Hi = rgulDen[1];

		for (;;) {
			sdlQuo.u.Lo--;
			sdlNum.int64 += sdlProd1.int64;
			rgulNum[2] += rgulDen[2];

			if (sdlNum.int64 < sdlProd1.int64) {
				// Detected carry. Check for carry out of top
				// before adding it in.
				//
				if (rgulNum[2]++ < rgulDen[2])
					break;
			}
			if (rgulNum[2] < rgulDen[2])
				break; // detected carry
		}
	}

	rgulNum[0] = sdlNum.u.Lo;
	rgulNum[1] = sdlNum.u.Hi;
	return sdlQuo.u.Lo;
}

Int32 DecDiv(Int32* d1, Int32* d2, Int32* res)
{
	DECIMAL* pdecL = (DECIMAL*)d1;
	DECIMAL* pdecR = (DECIMAL*)d2;
	DECIMAL* pdecRes = (DECIMAL*)res;

	UInt32   rgulQuo[3];
	UInt32   rgulQuoSave[3];
	UInt32   rgulRem[4];
	UInt32   rgulDivisor[3];
	UInt32   ulPwr;
	UInt32   ulTmp;
	UInt32   ulTmp1;
	SPLIT64  sdlTmp;
	SPLIT64  sdlDivisor;
	Int32    iScale;
	Int32    iCurScale;
	Byte     fUnscale;

	iScale = DECIMAL_SCALE(*pdecL) - DECIMAL_SCALE(*pdecR);
	fUnscale = 0;
	rgulDivisor[0] = DECIMAL_LO32(*pdecR);
	rgulDivisor[1] = DECIMAL_MID32(*pdecR);
	rgulDivisor[2] = DECIMAL_HI32(*pdecR);

	if (rgulDivisor[1] == 0 && rgulDivisor[2] == 0) {
		// Divisor is only 32 bits.  Easy divide.
		//
		if (rgulDivisor[0] == 0)
			return DISP_E_DIVBYZERO;

		rgulQuo[0] = DECIMAL_LO32(*pdecL);
		rgulQuo[1] = DECIMAL_MID32(*pdecL);
		rgulQuo[2] = DECIMAL_HI32(*pdecL);
		rgulRem[0] = Div96By32(rgulQuo, rgulDivisor[0]);

		for (;;) {
			if (rgulRem[0] == 0) {
				if (iScale < 0) {
					iCurScale = min(9, -iScale);
					goto HaveScale;
				}
				break;
			}
			// We need to unscale if and only if we have a non-zero remainder
			fUnscale = 1;

			// We have computed a quotient based on the natural scale 
			// ( <dividend scale> - <divisor scale> ).  We have a non-zero 
			// remainder, so now we should increase the scale if possible to 
			// include more quotient bits.
			// 
			// If it doesn't cause overflow, we'll loop scaling by 10^9 and 
			// computing more quotient bits as long as the remainder stays 
			// non-zero.  If scaling by that much would cause overflow, we'll 
			// drop out of the loop and scale by as much as we can.
			// 
			// Scaling by 10^9 will overflow if rgulQuo[2].rgulQuo[1] >= 2^32 / 10^9 
			// = 4.294 967 296.  So the upper limit is rgulQuo[2] == 4 and 
			// rgulQuo[1] == 0.294 967 296 * 2^32 = 1,266,874,889.7+.  Since 
			// quotient bits in rgulQuo[0] could be all 1's, then 1,266,874,888 
			// is the largest value in rgulQuo[1] (when rgulQuo[2] == 4) that is 
			// assured not to overflow.
			// 
			iCurScale = SearchScale(rgulQuo[2], rgulQuo[1], rgulQuo[0], iScale);
			if (iCurScale == 0) {
				// No more scaling to be done, but remainder is non-zero.
				// Round quotient.
				//
				ulTmp = rgulRem[0] << 1;
				if (ulTmp < rgulRem[0] || (ulTmp >= rgulDivisor[0] &&
					(ulTmp > rgulDivisor[0] || (rgulQuo[0] & 1)))) {
				RoundUp:
					if (!Add32To96(rgulQuo, 1)) {
						if (iScale == 0) {
							return DISP_E_OVERFLOW;
						}
						iScale--;
						OverflowUnscale(rgulQuo, 1);
						break;
					}
				}
				break;
			}

			if (iCurScale < 0)
				return DISP_E_OVERFLOW;

		HaveScale:
			ulPwr = ulPower10[iCurScale];
			iScale += iCurScale;

			if (IncreaseScale(rgulQuo, ulPwr) != 0)
				return DISP_E_OVERFLOW;

			sdlTmp.int64 = DivMod64by32(UInt32x32To64(rgulRem[0], ulPwr), rgulDivisor[0]);
			rgulRem[0] = sdlTmp.u.Hi;

			if (!Add32To96(rgulQuo, sdlTmp.u.Lo)) {
				if (iScale == 0) {
					return DISP_E_OVERFLOW;
				}
				iScale--;
				OverflowUnscale(rgulQuo, (rgulRem[0] != 0));
				break;
			}
		} // for (;;)
	}
	else {
		// Divisor has bits set in the upper 64 bits.
		//
		// Divisor must be fully normalized (shifted so bit 31 of the most 
		// significant UInt32 is 1).  Locate the MSB so we know how much to 
		// normalize by.  The dividend will be shifted by the same amount so 
		// the quotient is not changed.
		//
		if (rgulDivisor[2] == 0)
			ulTmp = rgulDivisor[1];
		else
			ulTmp = rgulDivisor[2];

		iCurScale = 0;
		if (!(ulTmp & 0xFFFF0000)) {
			iCurScale += 16;
			ulTmp <<= 16;
		}
		if (!(ulTmp & 0xFF000000)) {
			iCurScale += 8;
			ulTmp <<= 8;
		}
		if (!(ulTmp & 0xF0000000)) {
			iCurScale += 4;
			ulTmp <<= 4;
		}
		if (!(ulTmp & 0xC0000000)) {
			iCurScale += 2;
			ulTmp <<= 2;
		}
		if (!(ulTmp & 0x80000000)) {
			iCurScale++;
			ulTmp <<= 1;
		}

		// Shift both dividend and divisor left by iCurScale.
		// 
		sdlTmp.int64 = DECIMAL_LO64_GET(*pdecL) << iCurScale;
		rgulRem[0] = sdlTmp.u.Lo;
		rgulRem[1] = sdlTmp.u.Hi;
		sdlTmp.u.Lo = DECIMAL_MID32(*pdecL);
		sdlTmp.u.Hi = DECIMAL_HI32(*pdecL);
		sdlTmp.int64 <<= iCurScale;
		rgulRem[2] = sdlTmp.u.Hi;
		rgulRem[3] = (DECIMAL_HI32(*pdecL) >> (31 - iCurScale)) >> 1;

		sdlDivisor.u.Lo = rgulDivisor[0];
		sdlDivisor.u.Hi = rgulDivisor[1];
		sdlDivisor.int64 <<= iCurScale;

		if (rgulDivisor[2] == 0) {
			// Have a 64-bit divisor in sdlDivisor.  The remainder 
			// (currently 96 bits spread over 4 UInt32s) will be < divisor.
			// 
			sdlTmp.u.Lo = rgulRem[2];
			sdlTmp.u.Hi = rgulRem[3];

			rgulQuo[2] = 0;
			rgulQuo[1] = Div96By64(&rgulRem[1], sdlDivisor);
			rgulQuo[0] = Div96By64(rgulRem, sdlDivisor);

			for (;;) {
				if ((rgulRem[0] | rgulRem[1]) == 0) {
					if (iScale < 0) {
						iCurScale = min(9, -iScale);
						goto HaveScale64;
					}
					break;
				}

				// We need to unscale if and only if we have a non-zero remainder
				fUnscale = 1;

				// Remainder is non-zero.  Scale up quotient and remainder by 
				// powers of 10 so we can compute more significant bits.
				// 
				iCurScale = SearchScale(rgulQuo[2], rgulQuo[1], rgulQuo[0], iScale);
				if (iCurScale == 0) {
					// No more scaling to be done, but remainder is non-zero.
					// Round quotient.
					//
					sdlTmp.u.Lo = rgulRem[0];
					sdlTmp.u.Hi = rgulRem[1];
					if (sdlTmp.u.Hi >= 0x80000000 || (sdlTmp.int64 <<= 1) > sdlDivisor.int64 ||
						(sdlTmp.int64 == sdlDivisor.int64 && (rgulQuo[0] & 1)))
						goto RoundUp;
					break;
				}

				if (iCurScale < 0)
					return DISP_E_OVERFLOW;

			HaveScale64:
				ulPwr = ulPower10[iCurScale];
				iScale += iCurScale;

				if (IncreaseScale(rgulQuo, ulPwr) != 0)
					return DISP_E_OVERFLOW;

				rgulRem[2] = 0;  // rem is 64 bits, IncreaseScale uses 96
				IncreaseScale(rgulRem, ulPwr);
				ulTmp = Div96By64(rgulRem, sdlDivisor);
				if (!Add32To96(rgulQuo, ulTmp)) {
					if (iScale == 0) {
						return DISP_E_OVERFLOW;
					}
					iScale--;
					OverflowUnscale(rgulQuo, (rgulRem[0] != 0 || rgulRem[1] != 0));
					break;
				}

			} // for (;;)
		}
		else {
			// Have a 96-bit divisor in rgulDivisor[].
			//
			// Start by finishing the shift left by iCurScale.
			//
			sdlTmp.u.Lo = rgulDivisor[1];
			sdlTmp.u.Hi = rgulDivisor[2];
			sdlTmp.int64 <<= iCurScale;
			rgulDivisor[0] = sdlDivisor.u.Lo;
			rgulDivisor[1] = sdlDivisor.u.Hi;
			rgulDivisor[2] = sdlTmp.u.Hi;

			// The remainder (currently 96 bits spread over 4 UInt32s) 
			// will be < divisor.
			// 
			rgulQuo[2] = 0;
			rgulQuo[1] = 0;
			rgulQuo[0] = Div128By96(rgulRem, rgulDivisor);

			for (;;) {
				if ((rgulRem[0] | rgulRem[1] | rgulRem[2]) == 0) {
					if (iScale < 0) {
						iCurScale = min(9, -iScale);
						goto HaveScale96;
					}
					break;
				}

				// We need to unscale if and only if we have a non-zero remainder
				fUnscale = 1;

				// Remainder is non-zero.  Scale up quotient and remainder by 
				// powers of 10 so we can compute more significant bits.
				// 
				iCurScale = SearchScale(rgulQuo[2], rgulQuo[1], rgulQuo[0], iScale);
				if (iCurScale == 0) {
					// No more scaling to be done, but remainder is non-zero.
					// Round quotient.
					//
					if (rgulRem[2] >= 0x80000000)
						goto RoundUp;

					ulTmp = rgulRem[0] > 0x80000000;
					ulTmp1 = rgulRem[1] > 0x80000000;
					rgulRem[0] <<= 1;
					rgulRem[1] = (rgulRem[1] << 1) + ulTmp;
					rgulRem[2] = (rgulRem[2] << 1) + ulTmp1;

					if (rgulRem[2] > rgulDivisor[2] || rgulRem[2] == rgulDivisor[2] &&
						(rgulRem[1] > rgulDivisor[1] || rgulRem[1] == rgulDivisor[1] &&
						(rgulRem[0] > rgulDivisor[0] || rgulRem[0] == rgulDivisor[0] &&
						(rgulQuo[0] & 1))))
						goto RoundUp;
					break;
				}

				if (iCurScale < 0)
					return DISP_E_OVERFLOW;

			HaveScale96:
				ulPwr = ulPower10[iCurScale];
				iScale += iCurScale;

				if (IncreaseScale(rgulQuo, ulPwr) != 0)
					return DISP_E_OVERFLOW;

				rgulRem[3] = IncreaseScale(rgulRem, ulPwr);
				ulTmp = Div128By96(rgulRem, rgulDivisor);
				if (!Add32To96(rgulQuo, ulTmp)) {
					if (iScale == 0) {
						return DISP_E_OVERFLOW;
					}
					iScale--;
					OverflowUnscale(rgulQuo, (rgulRem[0] != 0 || rgulRem[1] != 0 || rgulRem[2] != 0 || rgulRem[3] != 0));
					break;
				}

			} // for (;;)
		}
	}

	// We need to unscale if and only if we have a non-zero remainder
	if (fUnscale) {
		// Try extracting any extra powers of 10 we may have 
		// added.  We do this by trying to divide out 10^8, 10^4, 10^2, and 10^1.
		// If a division by one of these powers returns a zero remainder, then
		// we keep the quotient.  If the remainder is not zero, then we restore
		// the previous value.
		// 
		// Since 10 = 2 * 5, there must be a factor of 2 for every power of 10
		// we can extract.  We use this as a quick test on whether to try a
		// given power.
		// 
		while ((rgulQuo[0] & 0xFF) == 0 && iScale >= 8) {
			rgulQuoSave[0] = rgulQuo[0];
			rgulQuoSave[1] = rgulQuo[1];
			rgulQuoSave[2] = rgulQuo[2];

			if (Div96By32(rgulQuoSave, 100000000) == 0) {
				rgulQuo[0] = rgulQuoSave[0];
				rgulQuo[1] = rgulQuoSave[1];
				rgulQuo[2] = rgulQuoSave[2];
				iScale -= 8;
			}
			else
				break;
		}

		if ((rgulQuo[0] & 0xF) == 0 && iScale >= 4) {
			rgulQuoSave[0] = rgulQuo[0];
			rgulQuoSave[1] = rgulQuo[1];
			rgulQuoSave[2] = rgulQuo[2];

			if (Div96By32(rgulQuoSave, 10000) == 0) {
				rgulQuo[0] = rgulQuoSave[0];
				rgulQuo[1] = rgulQuoSave[1];
				rgulQuo[2] = rgulQuoSave[2];
				iScale -= 4;
			}
		}

		if ((rgulQuo[0] & 3) == 0 && iScale >= 2) {
			rgulQuoSave[0] = rgulQuo[0];
			rgulQuoSave[1] = rgulQuo[1];
			rgulQuoSave[2] = rgulQuo[2];

			if (Div96By32(rgulQuoSave, 100) == 0) {
				rgulQuo[0] = rgulQuoSave[0];
				rgulQuo[1] = rgulQuoSave[1];
				rgulQuo[2] = rgulQuoSave[2];
				iScale -= 2;
			}
		}

		if ((rgulQuo[0] & 1) == 0 && iScale >= 1) {
			rgulQuoSave[0] = rgulQuo[0];
			rgulQuoSave[1] = rgulQuo[1];
			rgulQuoSave[2] = rgulQuo[2];

			if (Div96By32(rgulQuoSave, 10) == 0) {
				rgulQuo[0] = rgulQuoSave[0];
				rgulQuo[1] = rgulQuoSave[1];
				rgulQuo[2] = rgulQuoSave[2];
				iScale -= 1;
			}
		}
	}

	DECIMAL_HI32(*pdecRes) = rgulQuo[2];
	DECIMAL_MID32(*pdecRes) = rgulQuo[1];
	DECIMAL_LO32(*pdecRes) = rgulQuo[0];
	DECIMAL_SCALE(*pdecRes) = (Byte)iScale;
	DECIMAL_SIGN(*pdecRes) = DECIMAL_SIGN(*pdecL) ^ DECIMAL_SIGN(*pdecR);
	return NOERROR;
}