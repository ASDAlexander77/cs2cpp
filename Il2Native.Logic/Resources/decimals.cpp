// Decimals

#ifndef UInt32x32To64
#define UInt32x32To64(a, b) ((Int64)((Int32)(a)) * (Int64)((Int32)(b)))
#endif

typedef union {
	Int64 int64;
	struct {
#if BIGENDIAN
		Int32 Hi;
		Int32 Lo;
#else            
		Int32 Lo;
		Int32 Hi;
#endif           
	} u;
} SPLIT64;

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

#ifndef DECIMAL_LO64_GET
#define DECIMAL_LO64_GET(dec)       ((UInt64)(((UInt64)DECIMAL_MID32(dec) << 32) | DECIMAL_LO32(dec)))
#endif

#ifndef DECIMAL_LO64_SET
#define DECIMAL_LO64_SET(dec,value)   {UInt64 _Value = value; DECIMAL_MID32(dec) = (UInt32)(_Value >> 32); DECIMAL_LO32(dec) = (UInt32)_Value; }
#endif

#define NOERROR 0
#define DISP_E_OVERFLOW 1

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


Int32 rgulPower10[POWER10_MAX + 1] = { 1, 10, 100, 1000, 10000, 100000, 1000000,
10000000, 100000000, 1000000000 };

struct DECOVFL
{
	Int32 Hi;
	Int32 Mid;
	Int32 Lo;
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

static const UInt32 ulTenToNine = 1000000000;
#define COPYDEC(dest, src) {DECIMAL_SIGNSCALE(dest) = DECIMAL_SIGNSCALE(src); DECIMAL_HI32(dest) = DECIMAL_HI32(src); DECIMAL_LO64_SET(dest, DECIMAL_LO64_GET(src));}

#define Div64by32(num, den) ((UInt32)((UInt64)(num) / (UInt32)(den)))
#define Mod64by32(num, den) ((UInt32)((UInt64)(num) % (UInt32)(den)))

inline UInt64 DivMod64by32(UInt64 num, UInt32 den)
{
	SPLIT64  sdl;

	sdl.u.Lo = Div64by32(num, den);
	sdl.u.Hi = Mod64by32(num, den);
	return sdl.int64;
}

int SearchScale(UInt32 ulResHi, UInt32 ulResMid, UInt32 ulResLo, int iScale)
{
	int   iCurScale;

	// Quick check to stop us from trying to scale any more.
	//
	if (ulResHi > OVFL_MAX_1_HI || iScale >= DEC_SCALE_MAX) {
		iCurScale = 0;
		goto HaveScale;
	}

	if (iScale > DEC_SCALE_MAX - 9) {
		// We can't scale by 10^9 without exceeding the max scale factor.
		// See if we can scale to the max.  If not, we'll fall into
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

int ScaleResult(UInt32 *rgulRes, int iHiRes, int iScale)
{
	int     iNewScale;
	int     iCur;
	UInt32   ulPwr;
	UInt32   ulTmp;
	UInt32   ulSticky;
	SPLIT64 sdlTmp;

	// See if we need to scale the result.  The combined scale must
	// be <= DEC_SCALE_MAX and the upper 96 bits must be zero.
	// 
	// Start by figuring a lower bound on the scaling needed to make
	// the upper 96 bits zero.  iHiRes is the index into rgulRes[]
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
		// to make it's integer part zero after dividing by 256.
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
	// into valid range.
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
				ulPwr = rgulPower10[iNewScale];

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
	int       iScale;
	int       iHiProd;
	int       iCur;
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
			ulPwr = rgulPower10[iScale];
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
					ulPwr = rgulPower10[iScale];

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

#define VARCMP_EQ 0
#define VARCMP_LT -1
#define VARCMP_GT 1

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


