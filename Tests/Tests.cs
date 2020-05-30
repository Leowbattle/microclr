using microclr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Tests
{
	[TestClass]
	public class Tests
	{
		#region Suppress warnings
#pragma warning disable CS0219 // Variable assigned but not used
#pragma warning disable IDE0059// Variable assigned but not used
#pragma warning disable CS0162 // Unreachable code
#pragma warning disable HAA0601
#pragma warning disable HAA0101
		#endregion

		#region Test executor
		static object Run(string name, params object[] args)
		{
			var method = typeof(Tests).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
			return new MicroClr().Execute(method, args);
		}

		static T Run<T>(string name, params object[] args) where T : unmanaged
		{
			var method = typeof(Tests).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
			return new MicroClr().Execute<T>(method, args);
		}

		static void RunTest(string name, params object[] args)
		{
			var method = typeof(Tests).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
			var dotnet = method.Invoke(null, args);
			var microclr = new MicroClr().Execute(method, args);
			Assert.AreEqual(dotnet, microclr);
		}

		static void RunTest(Delegate d, params object[] args)
		{
			var dotnet = d.DynamicInvoke(args);
			var microclr = new MicroClr().Execute(d.Method, args);
			Assert.AreEqual(dotnet, microclr);
		}
		#endregion

		#region Empty method
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static void EmptyMethod()
		{

		}

		[TestMethod]
		public void TestEmptyMethod()
		{
			RunTest(nameof(EmptyMethod));
		}
		#endregion

		#region Local variables
		/// <summary>
		/// There are 256 local variables in this function, so it uses all of the local variable storing instructions
		/// (Stloc, Stloc_S, Stloc_0, Stloc_1, Stloc_2, Stloc_3)
		/// </summary>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static void LocalVariables()
		{
			int local0 = 0;
			int local1 = 1;
			int local2 = 2;
			int local3 = 3;
			int local4 = 4;
			int local5 = 5;
			int local6 = 6;
			int local7 = 7;
			int local8 = 8;
			int local9 = 9;
			int local10 = 10;
			int local11 = 11;
			int local12 = 12;
			int local13 = 13;
			int local14 = 14;
			int local15 = 15;
			int local16 = 16;
			int local17 = 17;
			int local18 = 18;
			int local19 = 19;
			int local20 = 20;
			int local21 = 21;
			int local22 = 22;
			int local23 = 23;
			int local24 = 24;
			int local25 = 25;
			int local26 = 26;
			int local27 = 27;
			int local28 = 28;
			int local29 = 29;
			int local30 = 30;
			int local31 = 31;
			int local32 = 32;
			int local33 = 33;
			int local34 = 34;
			int local35 = 35;
			int local36 = 36;
			int local37 = 37;
			int local38 = 38;
			int local39 = 39;
			int local40 = 40;
			int local41 = 41;
			int local42 = 42;
			int local43 = 43;
			int local44 = 44;
			int local45 = 45;
			int local46 = 46;
			int local47 = 47;
			int local48 = 48;
			int local49 = 49;
			int local50 = 50;
			int local51 = 51;
			int local52 = 52;
			int local53 = 53;
			int local54 = 54;
			int local55 = 55;
			int local56 = 56;
			int local57 = 57;
			int local58 = 58;
			int local59 = 59;
			int local60 = 60;
			int local61 = 61;
			int local62 = 62;
			int local63 = 63;
			int local64 = 64;
			int local65 = 65;
			int local66 = 66;
			int local67 = 67;
			int local68 = 68;
			int local69 = 69;
			int local70 = 70;
			int local71 = 71;
			int local72 = 72;
			int local73 = 73;
			int local74 = 74;
			int local75 = 75;
			int local76 = 76;
			int local77 = 77;
			int local78 = 78;
			int local79 = 79;
			int local80 = 80;
			int local81 = 81;
			int local82 = 82;
			int local83 = 83;
			int local84 = 84;
			int local85 = 85;
			int local86 = 86;
			int local87 = 87;
			int local88 = 88;
			int local89 = 89;
			int local90 = 90;
			int local91 = 91;
			int local92 = 92;
			int local93 = 93;
			int local94 = 94;
			int local95 = 95;
			int local96 = 96;
			int local97 = 97;
			int local98 = 98;
			int local99 = 99;
			int local100 = 100;
			int local101 = 101;
			int local102 = 102;
			int local103 = 103;
			int local104 = 104;
			int local105 = 105;
			int local106 = 106;
			int local107 = 107;
			int local108 = 108;
			int local109 = 109;
			int local110 = 110;
			int local111 = 111;
			int local112 = 112;
			int local113 = 113;
			int local114 = 114;
			int local115 = 115;
			int local116 = 116;
			int local117 = 117;
			int local118 = 118;
			int local119 = 119;
			int local120 = 120;
			int local121 = 121;
			int local122 = 122;
			int local123 = 123;
			int local124 = 124;
			int local125 = 125;
			int local126 = 126;
			int local127 = 127;
			int local128 = 128;
			int local129 = 129;
			int local130 = 130;
			int local131 = 131;
			int local132 = 132;
			int local133 = 133;
			int local134 = 134;
			int local135 = 135;
			int local136 = 136;
			int local137 = 137;
			int local138 = 138;
			int local139 = 139;
			int local140 = 140;
			int local141 = 141;
			int local142 = 142;
			int local143 = 143;
			int local144 = 144;
			int local145 = 145;
			int local146 = 146;
			int local147 = 147;
			int local148 = 148;
			int local149 = 149;
			int local150 = 150;
			int local151 = 151;
			int local152 = 152;
			int local153 = 153;
			int local154 = 154;
			int local155 = 155;
			int local156 = 156;
			int local157 = 157;
			int local158 = 158;
			int local159 = 159;
			int local160 = 160;
			int local161 = 161;
			int local162 = 162;
			int local163 = 163;
			int local164 = 164;
			int local165 = 165;
			int local166 = 166;
			int local167 = 167;
			int local168 = 168;
			int local169 = 169;
			int local170 = 170;
			int local171 = 171;
			int local172 = 172;
			int local173 = 173;
			int local174 = 174;
			int local175 = 175;
			int local176 = 176;
			int local177 = 177;
			int local178 = 178;
			int local179 = 179;
			int local180 = 180;
			int local181 = 181;
			int local182 = 182;
			int local183 = 183;
			int local184 = 184;
			int local185 = 185;
			int local186 = 186;
			int local187 = 187;
			int local188 = 188;
			int local189 = 189;
			int local190 = 190;
			int local191 = 191;
			int local192 = 192;
			int local193 = 193;
			int local194 = 194;
			int local195 = 195;
			int local196 = 196;
			int local197 = 197;
			int local198 = 198;
			int local199 = 199;
			int local200 = 200;
			int local201 = 201;
			int local202 = 202;
			int local203 = 203;
			int local204 = 204;
			int local205 = 205;
			int local206 = 206;
			int local207 = 207;
			int local208 = 208;
			int local209 = 209;
			int local210 = 210;
			int local211 = 211;
			int local212 = 212;
			int local213 = 213;
			int local214 = 214;
			int local215 = 215;
			int local216 = 216;
			int local217 = 217;
			int local218 = 218;
			int local219 = 219;
			int local220 = 220;
			int local221 = 221;
			int local222 = 222;
			int local223 = 223;
			int local224 = 224;
			int local225 = 225;
			int local226 = 226;
			int local227 = 227;
			int local228 = 228;
			int local229 = 229;
			int local230 = 230;
			int local231 = 231;
			int local232 = 232;
			int local233 = 233;
			int local234 = 234;
			int local235 = 235;
			int local236 = 236;
			int local237 = 237;
			int local238 = 238;
			int local239 = 239;
			int local240 = 240;
			int local241 = 241;
			int local242 = 242;
			int local243 = 243;
			int local244 = 244;
			int local245 = 245;
			int local246 = 246;
			int local247 = 247;
			int local248 = 248;
			int local249 = 249;
			int local250 = 250;
			int local251 = 251;
			int local252 = 252;
			int local253 = 253;
			int local254 = 254;
			int local255 = 255;
		}

		[TestMethod]
		public void TestLocalVariables()
		{
			RunTest(nameof(LocalVariables));
		}
		#endregion

		#region Simple returns
		/// <summary>
		/// Returns an int using Ldc_I4_S
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int ReturnSmallInt()
		{
			return 42;
		}

		/// <summary>
		/// Returns an int using Ldc_I4
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int ReturnLargeInt()
		{
			return 1000000;
		}

		/// <summary>
		/// Returns a float using Ldc_R4
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static float ReturnFloat()
		{
			return 3.14159f;
		}

		/// <summary>
		/// Returns a double using Ldc_R8
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static double ReturnDouble()
		{
			return 2.71828;
		}

		[TestMethod]
		public void TestSimpleReturn()
		{
			RunTest(nameof(ReturnSmallInt));
			RunTest(nameof(ReturnLargeInt));
			RunTest(nameof(ReturnFloat));
			RunTest(nameof(ReturnDouble));
		}

		[TestMethod]
		[ExpectedException(typeof(IncorrectReturnTypeException))]
		public void TestIncorrectReturnType()
		{
			Run<int>(nameof(ReturnFloat));
		}

		[TestMethod]
		[ExpectedException(typeof(IncorrectReturnTypeException))]
		public void TestVoidReturnType()
		{
			Assert.AreEqual(Run(nameof(EmptyMethod)), null);
			Run<int>(nameof(EmptyMethod));
		}
		#endregion

		#region Small int constants
		/// <summary>
		/// Loads ints using Ldc_I4_[0 to 8] and Ldc_I4_M1
		/// </summary>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static void LoadSmallIntConstants()
		{
			int x = -1;
			x = 0;
			x = 1;
			x = 2;
			x = 3;
			x = 4;
			x = 5;
			x = 6;
			x = 7;
			x = 8;
		}

		[TestMethod]
		public void TestSmallIntConstantLoading()
		{
			RunTest(nameof(LoadSmallIntConstants));
		}
		#endregion

		#region Unconditional jump
		/// <summary>
		/// Unconditional jump using Br_S
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static bool UnconditionalJump()
		{
			goto label1;
			return true;
			label1:
			return false;
		}

		/// <summary>
		/// Unconditional jump using Br
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static bool FarUnconditionalJump()
		{
			goto farLabel;

			label2:
			int local0 = 0;
			int local1 = 1;
			int local2 = 2;
			int local3 = 3;
			int local4 = 4;
			int local5 = 5;
			int local6 = 6;
			int local7 = 7;
			int local8 = 8;
			int local9 = 9;
			int local10 = 10;
			int local11 = 11;
			int local12 = 12;
			int local13 = 13;
			int local14 = 14;
			int local15 = 15;
			int local16 = 16;
			int local17 = 17;
			int local18 = 18;
			int local19 = 19;
			int local20 = 20;
			int local21 = 21;
			int local22 = 22;
			int local23 = 23;
			int local24 = 24;
			int local25 = 25;
			int local26 = 26;
			int local27 = 27;
			int local28 = 28;
			int local29 = 29;
			int local30 = 30;
			int local31 = 31;
			int local32 = 32;
			int local33 = 33;
			int local34 = 34;
			int local35 = 35;
			int local36 = 36;
			int local37 = 37;
			int local38 = 38;
			int local39 = 39;
			int local40 = 40;
			int local41 = 41;
			int local42 = 42;
			int local43 = 43;
			int local44 = 44;
			int local45 = 45;
			int local46 = 46;
			int local47 = 47;
			int local48 = 48;
			int local49 = 49;
			int local50 = 50;
			int local51 = 51;
			int local52 = 52;
			int local53 = 53;
			int local54 = 54;
			int local55 = 55;
			int local56 = 56;
			int local57 = 57;
			int local58 = 58;
			int local59 = 59;
			int local60 = 60;
			int local61 = 61;
			int local62 = 62;
			int local63 = 63;
			int local64 = 64;
			return true;

			farLabel:
			goto label2;
			return false;
		}

		[TestMethod]
		public void TestUnconditionalJump()
		{
			RunTest(nameof(UnconditionalJump));
			RunTest(nameof(FarUnconditionalJump));
		}
		#endregion

		#region Int add
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int SimpleAdd()
		{
			int a = 1;
			int b = 2;
			return a + b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static uint SimpleAddUInt()
		{
			uint a = 5;
			uint b = 6;
			return a + b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int AddIntOverflow()
		{
			int a = 2000000000;
			int b = 2000000000;
			return a + b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static uint AddUIntOverflow()
		{
			uint a = 4000000000;
			uint b = 4000000000;
			return a + b;
		}

		[TestMethod]
		public void TestIntAdd()
		{
			RunTest(nameof(SimpleAdd));
			RunTest(nameof(SimpleAddUInt));
			RunTest(nameof(AddIntOverflow));
			RunTest(nameof(AddUIntOverflow));
		}
		#endregion

		#region Int subtract
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int SimpleSubtract()
		{
			int a = 1;
			int b = 2;
			return a - b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static uint SimpleSubtractUInt()
		{
			uint a = 6;
			uint b = 5;
			return a - b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int SubtractIntUnderflow()
		{
			int a = 0;
			int b = 2000000000;
			return a - b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static uint SubtractUIntUnderflow()
		{
			uint a = 0;
			uint b = 4000000000;
			return a - b;
		}

		[TestMethod]
		public void TestIntSubtract()
		{
			RunTest(nameof(SimpleSubtract));
			RunTest(nameof(SimpleSubtractUInt));
			RunTest(nameof(SubtractIntUnderflow));
			RunTest(nameof(SubtractUIntUnderflow));
		}
		#endregion

		#region Float add
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static float SimpleAddFloat()
		{
			float a = 0.1f;
			float b = 0.2f;
			return a + b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static float AddFloatOverflow()
		{
			float a = float.MaxValue;
			float b = float.MaxValue;
			return a + b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static double SimpleAddDouble()
		{
			double a = 0.1;
			double b = 0.2;
			return a + b;
		}

		[TestMethod]
		public void TestFloatAdd()
		{
			RunTest(nameof(SimpleAddFloat));
			RunTest(nameof(SimpleAddDouble));
			RunTest(nameof(AddFloatOverflow));
		}
		#endregion

		#region Float subtract
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static float SimpleSubtractFloat()
		{
			float a = 0.1f;
			float b = 0.2f;
			return a - b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static float SubtractFloatOverflow()
		{
			float a = float.MinValue;
			float b = float.MinValue;
			return a - b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static double SimpleSubtractDouble()
		{
			double a = 0.1;
			double b = 0.2;
			return a - b;
		}

		[TestMethod]
		public void TestFloatSubtract()
		{
			RunTest(nameof(SimpleSubtractFloat));
			RunTest(nameof(SimpleSubtractDouble));
			RunTest(nameof(SubtractFloatOverflow));
		}
		#endregion

		#region Int multiply
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int SimpleIntMultiply()
		{
			int a = 6;
			int b = 7;
			return a * b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int IntMultiplyNegative()
		{
			int a = 1;
			int b = -7;
			return a * b;
		}

		[TestMethod]
		public void TestIntMultiply()
		{
			RunTest(nameof(SimpleIntMultiply));
			RunTest(nameof(IntMultiplyNegative));
		}
		#endregion

		#region Float multiply
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static float SimpleFloatMultiply()
		{
			float a = -6;
			float b = 3.14159f;
			return a * b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static double SimpleDoubleMultiply()
		{
			double a = -4;
			double b = 2.71828;
			return a * b;
		}

		[TestMethod]
		public void TestFloatMultiply()
		{
			RunTest(nameof(SimpleFloatMultiply));
			RunTest(nameof(SimpleDoubleMultiply));
		}
		#endregion

		#region Int divide
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int SimpleDivide()
		{
			int a = 4;
			int b = -4;
			return a / b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int DivideByLargerNumber()
		{
			int a = 100;
			int b = 12345;
			return a / b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int DivideByZero()
		{
			int a = 42;
			int b = 0;
			return a / b;
		}

		[TestMethod]
		[ExpectedException(typeof(DivideByZeroException))]
		public void TestIntegerDivision()
		{
			RunTest(nameof(SimpleDivide));
			RunTest(nameof(DivideByLargerNumber));
			Run<int>(nameof(DivideByZero));
		}
		#endregion

		#region Float divide
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static float SimpleDivideFloat()
		{
			float a = 1;
			float b = -3;
			return a / b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static float FloatDivideByZero()
		{
			float a = 10;
			float b = 0;
			return a / b;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static double SimpleDivideDouble()
		{
			double a = 1;
			double b = 3.14159;
			return a / b;
		}

		[TestMethod]
		public void TestFloatDivide()
		{
			RunTest(nameof(SimpleDivideFloat));
			RunTest(nameof(FloatDivideByZero));
			RunTest(nameof(SimpleDivideDouble));
		}
		#endregion

		#region Negation
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int NegateInt()
		{
			int a = 5;
			return -a;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static float NegateFloat()
		{
			float f = 3;
			return -f;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static double NegateDouble()
		{
			double d = 42;
			return -d;
		}

		[TestMethod]
		public void TestNegation()
		{
			RunTest(nameof(NegateInt));
			RunTest(nameof(NegateFloat));
			RunTest(nameof(NegateDouble));
		}
		#endregion

		#region Parameters
		[TestMethod]
		[ExpectedException(typeof(ParameterCountException))]
		public void TestIncorrectParameterCount()
		{
			Run(nameof(EmptyMethod), 1, 2, 3);
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static void Takes2Ints(int a, int b)
		{

		}

		[TestMethod]
		[ExpectedException(typeof(ParameterTypeException))]
		public void TestIncorrectParameterType()
		{
			Run(nameof(Takes2Ints), 1, 1.0);
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int Add(int a, int b)
		{
			return a + b;
		}

		/// <summary>
		/// Loads parameters using Ldarg, Ldarg_S, Ldarg_0, Ldarg_1, Ldarg_2, Ldarg_3
		/// </summary>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static void HasManyParameters(
			int arg0,
			int arg1,
			int arg2,
			int arg3,
			int arg4,
			int arg5,
			int arg6,
			int arg7,
			int arg8,
			int arg9,
			int arg10,
			int arg11,
			int arg12,
			int arg13,
			int arg14,
			int arg15,
			int arg16,
			int arg17,
			int arg18,
			int arg19,
			int arg20,
			int arg21,
			int arg22,
			int arg23,
			int arg24,
			int arg25,
			int arg26,
			int arg27,
			int arg28,
			int arg29,
			int arg30,
			int arg31,
			int arg32,
			int arg33,
			int arg34,
			int arg35,
			int arg36,
			int arg37,
			int arg38,
			int arg39,
			int arg40,
			int arg41,
			int arg42,
			int arg43,
			int arg44,
			int arg45,
			int arg46,
			int arg47,
			int arg48,
			int arg49,
			int arg50,
			int arg51,
			int arg52,
			int arg53,
			int arg54,
			int arg55,
			int arg56,
			int arg57,
			int arg58,
			int arg59,
			int arg60,
			int arg61,
			int arg62,
			int arg63,
			int arg64,
			int arg65,
			int arg66,
			int arg67,
			int arg68,
			int arg69,
			int arg70,
			int arg71,
			int arg72,
			int arg73,
			int arg74,
			int arg75,
			int arg76,
			int arg77,
			int arg78,
			int arg79,
			int arg80,
			int arg81,
			int arg82,
			int arg83,
			int arg84,
			int arg85,
			int arg86,
			int arg87,
			int arg88,
			int arg89,
			int arg90,
			int arg91,
			int arg92,
			int arg93,
			int arg94,
			int arg95,
			int arg96,
			int arg97,
			int arg98,
			int arg99,
			int arg100,
			int arg101,
			int arg102,
			int arg103,
			int arg104,
			int arg105,
			int arg106,
			int arg107,
			int arg108,
			int arg109,
			int arg110,
			int arg111,
			int arg112,
			int arg113,
			int arg114,
			int arg115,
			int arg116,
			int arg117,
			int arg118,
			int arg119,
			int arg120,
			int arg121,
			int arg122,
			int arg123,
			int arg124,
			int arg125,
			int arg126,
			int arg127,
			int arg128,
			int arg129,
			int arg130,
			int arg131,
			int arg132,
			int arg133,
			int arg134,
			int arg135,
			int arg136,
			int arg137,
			int arg138,
			int arg139,
			int arg140,
			int arg141,
			int arg142,
			int arg143,
			int arg144,
			int arg145,
			int arg146,
			int arg147,
			int arg148,
			int arg149,
			int arg150,
			int arg151,
			int arg152,
			int arg153,
			int arg154,
			int arg155,
			int arg156,
			int arg157,
			int arg158,
			int arg159,
			int arg160,
			int arg161,
			int arg162,
			int arg163,
			int arg164,
			int arg165,
			int arg166,
			int arg167,
			int arg168,
			int arg169,
			int arg170,
			int arg171,
			int arg172,
			int arg173,
			int arg174,
			int arg175,
			int arg176,
			int arg177,
			int arg178,
			int arg179,
			int arg180,
			int arg181,
			int arg182,
			int arg183,
			int arg184,
			int arg185,
			int arg186,
			int arg187,
			int arg188,
			int arg189,
			int arg190,
			int arg191,
			int arg192,
			int arg193,
			int arg194,
			int arg195,
			int arg196,
			int arg197,
			int arg198,
			int arg199,
			int arg200,
			int arg201,
			int arg202,
			int arg203,
			int arg204,
			int arg205,
			int arg206,
			int arg207,
			int arg208,
			int arg209,
			int arg210,
			int arg211,
			int arg212,
			int arg213,
			int arg214,
			int arg215,
			int arg216,
			int arg217,
			int arg218,
			int arg219,
			int arg220,
			int arg221,
			int arg222,
			int arg223,
			int arg224,
			int arg225,
			int arg226,
			int arg227,
			int arg228,
			int arg229,
			int arg230,
			int arg231,
			int arg232,
			int arg233,
			int arg234,
			int arg235,
			int arg236,
			int arg237,
			int arg238,
			int arg239,
			int arg240,
			int arg241,
			int arg242,
			int arg243,
			int arg244,
			int arg245,
			int arg246,
			int arg247,
			int arg248,
			int arg249,
			int arg250,
			int arg251,
			int arg252,
			int arg253,
			int arg254,
			int arg255)
		{
			int x = arg0;
			x = arg1;
			x = arg2;
			x = arg3;
			x = arg4;
			x = arg5;
			x = arg6;
			x = arg7;
			x = arg8;
			x = arg9;
			x = arg10;
			x = arg11;
			x = arg12;
			x = arg13;
			x = arg14;
			x = arg15;
			x = arg16;
			x = arg17;
			x = arg18;
			x = arg19;
			x = arg20;
			x = arg21;
			x = arg22;
			x = arg23;
			x = arg24;
			x = arg25;
			x = arg26;
			x = arg27;
			x = arg28;
			x = arg29;
			x = arg30;
			x = arg31;
			x = arg32;
			x = arg33;
			x = arg34;
			x = arg35;
			x = arg36;
			x = arg37;
			x = arg38;
			x = arg39;
			x = arg40;
			x = arg41;
			x = arg42;
			x = arg43;
			x = arg44;
			x = arg45;
			x = arg46;
			x = arg47;
			x = arg48;
			x = arg49;
			x = arg50;
			x = arg51;
			x = arg52;
			x = arg53;
			x = arg54;
			x = arg55;
			x = arg56;
			x = arg57;
			x = arg58;
			x = arg59;
			x = arg60;
			x = arg61;
			x = arg62;
			x = arg63;
			x = arg64;
			x = arg65;
			x = arg66;
			x = arg67;
			x = arg68;
			x = arg69;
			x = arg70;
			x = arg71;
			x = arg72;
			x = arg73;
			x = arg74;
			x = arg75;
			x = arg76;
			x = arg77;
			x = arg78;
			x = arg79;
			x = arg80;
			x = arg81;
			x = arg82;
			x = arg83;
			x = arg84;
			x = arg85;
			x = arg86;
			x = arg87;
			x = arg88;
			x = arg89;
			x = arg90;
			x = arg91;
			x = arg92;
			x = arg93;
			x = arg94;
			x = arg95;
			x = arg96;
			x = arg97;
			x = arg98;
			x = arg99;
			x = arg100;
			x = arg101;
			x = arg102;
			x = arg103;
			x = arg104;
			x = arg105;
			x = arg106;
			x = arg107;
			x = arg108;
			x = arg109;
			x = arg110;
			x = arg111;
			x = arg112;
			x = arg113;
			x = arg114;
			x = arg115;
			x = arg116;
			x = arg117;
			x = arg118;
			x = arg119;
			x = arg120;
			x = arg121;
			x = arg122;
			x = arg123;
			x = arg124;
			x = arg125;
			x = arg126;
			x = arg127;
			x = arg128;
			x = arg129;
			x = arg130;
			x = arg131;
			x = arg132;
			x = arg133;
			x = arg134;
			x = arg135;
			x = arg136;
			x = arg137;
			x = arg138;
			x = arg139;
			x = arg140;
			x = arg141;
			x = arg142;
			x = arg143;
			x = arg144;
			x = arg145;
			x = arg146;
			x = arg147;
			x = arg148;
			x = arg149;
			x = arg150;
			x = arg151;
			x = arg152;
			x = arg153;
			x = arg154;
			x = arg155;
			x = arg156;
			x = arg157;
			x = arg158;
			x = arg159;
			x = arg160;
			x = arg161;
			x = arg162;
			x = arg163;
			x = arg164;
			x = arg165;
			x = arg166;
			x = arg167;
			x = arg168;
			x = arg169;
			x = arg170;
			x = arg171;
			x = arg172;
			x = arg173;
			x = arg174;
			x = arg175;
			x = arg176;
			x = arg177;
			x = arg178;
			x = arg179;
			x = arg180;
			x = arg181;
			x = arg182;
			x = arg183;
			x = arg184;
			x = arg185;
			x = arg186;
			x = arg187;
			x = arg188;
			x = arg189;
			x = arg190;
			x = arg191;
			x = arg192;
			x = arg193;
			x = arg194;
			x = arg195;
			x = arg196;
			x = arg197;
			x = arg198;
			x = arg199;
			x = arg200;
			x = arg201;
			x = arg202;
			x = arg203;
			x = arg204;
			x = arg205;
			x = arg206;
			x = arg207;
			x = arg208;
			x = arg209;
			x = arg210;
			x = arg211;
			x = arg212;
			x = arg213;
			x = arg214;
			x = arg215;
			x = arg216;
			x = arg217;
			x = arg218;
			x = arg219;
			x = arg220;
			x = arg221;
			x = arg222;
			x = arg223;
			x = arg224;
			x = arg225;
			x = arg226;
			x = arg227;
			x = arg228;
			x = arg229;
			x = arg230;
			x = arg231;
			x = arg232;
			x = arg233;
			x = arg234;
			x = arg235;
			x = arg236;
			x = arg237;
			x = arg238;
			x = arg239;
			x = arg240;
			x = arg241;
			x = arg242;
			x = arg243;
			x = arg244;
			x = arg245;
			x = arg246;
			x = arg247;
			x = arg248;
			x = arg249;
			x = arg250;
			x = arg251;
			x = arg252;
			x = arg253;
			x = arg254;
			x = arg255;
		}

		[TestMethod]
		public void TestSimpleArguments()
		{
			RunTest(nameof(Add), 5, 2);
			RunTest(
				nameof(HasManyParameters),
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15,
				16,
				17,
				18,
				19,
				20,
				21,
				22,
				23,
				24,
				25,
				26,
				27,
				28,
				29,
				30,
				31,
				32,
				33,
				34,
				35,
				36,
				37,
				38,
				39,
				40,
				41,
				42,
				43,
				44,
				45,
				46,
				47,
				48,
				49,
				50,
				51,
				52,
				53,
				54,
				55,
				56,
				57,
				58,
				59,
				60,
				61,
				62,
				63,
				64,
				65,
				66,
				67,
				68,
				69,
				70,
				71,
				72,
				73,
				74,
				75,
				76,
				77,
				78,
				79,
				80,
				81,
				82,
				83,
				84,
				85,
				86,
				87,
				88,
				89,
				90,
				91,
				92,
				93,
				94,
				95,
				96,
				97,
				98,
				99,
				100,
				101,
				102,
				103,
				104,
				105,
				106,
				107,
				108,
				109,
				110,
				111,
				112,
				113,
				114,
				115,
				116,
				117,
				118,
				119,
				120,
				121,
				122,
				123,
				124,
				125,
				126,
				127,
				128,
				129,
				130,
				131,
				132,
				133,
				134,
				135,
				136,
				137,
				138,
				139,
				140,
				141,
				142,
				143,
				144,
				145,
				146,
				147,
				148,
				149,
				150,
				151,
				152,
				153,
				154,
				155,
				156,
				157,
				158,
				159,
				160,
				161,
				162,
				163,
				164,
				165,
				166,
				167,
				168,
				169,
				170,
				171,
				172,
				173,
				174,
				175,
				176,
				177,
				178,
				179,
				180,
				181,
				182,
				183,
				184,
				185,
				186,
				187,
				188,
				189,
				190,
				191,
				192,
				193,
				194,
				195,
				196,
				197,
				198,
				199,
				200,
				201,
				202,
				203,
				204,
				205,
				206,
				207,
				208,
				209,
				210,
				211,
				212,
				213,
				214,
				215,
				216,
				217,
				218,
				219,
				220,
				221,
				222,
				223,
				224,
				225,
				226,
				227,
				228,
				229,
				230,
				231,
				232,
				233,
				234,
				235,
				236,
				237,
				238,
				239,
				240,
				241,
				242,
				243,
				244,
				245,
				246,
				247,
				248,
				249,
				250,
				251,
				252,
				253,
				254,
				255);
		}
		#endregion

		#region Comparisons, conditional jumps
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static bool AreEqual(int x, int y)
		{
			return x == y;
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int Fibonacci(int n)
		{
			int a = 0;
			int b = 1;
			int c = 1;

			for (int i = 0; i < n - 1; i++)
			{
				c = a + b;
				a = b;
				b = c;
			}

			return c;
		}

		[TestMethod]
		public void TestConditionalJump()
		{
			RunTest(nameof(AreEqual), 5, 5);
			RunTest(nameof(AreEqual), 5, 4);
			RunTest(nameof(Fibonacci), 10);
		}
		#endregion

		#region Switch

		/// <summary>
		/// The IL for this switch statement will be generated as an if-else if chain.
		/// </summary>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int Switch1(int n)
		{
			return n switch
			{
				1 => 42,
				3 => 44,
				_ => n
			};
		}

		/// <summary>
		/// This switch statement is generated using the CIL switch instruction.
		/// It seems to use this instruction if each case is within 3 of the previous one.
		/// The compiler was even smart enough to subtract 1 from n before the switch instruction
		/// so it starts at 0 and is able to use a jump-table.
		/// </summary>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		static int Switch2(int n)
		{
			return n switch
			{
				2 => 2,
				3 => 3,
				4 => 50,
				_ => n
			};
		}

		[TestMethod]
		public void TestSwitch()
		{
			RunTest(nameof(Switch1), 0);
			RunTest(nameof(Switch1), 1);
			RunTest(nameof(Switch1), 3);

			RunTest(nameof(Switch2), 0);
			RunTest(nameof(Switch2), 1);
			RunTest(nameof(Switch2), 2);
			RunTest(nameof(Switch2), 3);
			RunTest(nameof(Switch2), 4);
			RunTest(nameof(Switch2), 5);
			RunTest(nameof(Switch2), 50);
		}
		#endregion
	}
}
