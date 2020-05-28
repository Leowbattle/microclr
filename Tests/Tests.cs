using microclr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Tests
{
	[TestClass]
	public class Tests
	{
		static void Run(string name)
		{
			var method = typeof(Tests).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
			new MicroClr().Execute(method);
		}

		static T Run<T>(string name) where T : unmanaged
		{
			var method = typeof(Tests).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
			return new MicroClr().Execute<T>(method);
		}

		static void EmptyMethod()
		{

		}

		[TestMethod]
		public void TestEmptyMethod()
		{
			Run(nameof(EmptyMethod));
		}

		/// <summary>
		/// There are 256 local variables in this function, so it uses all of the local variable storing instructions
		/// (Stloc, Stloc_S, Stloc_0, Stloc_1, Stloc_2, Stloc_3)
		/// </summary>
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
			Run(nameof(LocalVariables));
		}

		/// <summary>
		/// Returns an int using Ldc_I4_S
		/// </summary>
		/// <returns></returns>
		static int ReturnSmallInt()
		{
			return 42;
		}

		/// <summary>
		/// Returns an int using Ldc_I4
		/// </summary>
		/// <returns></returns>
		static int ReturnLargeInt()
		{
			return 1000000;
		}

		/// <summary>
		/// Returns a float using Ldc_R4
		/// </summary>
		/// <returns></returns>
		static float ReturnFloat()
		{
			return 3.14159f;
		}

		/// <summary>
		/// Returns a double using Ldc_R8
		/// </summary>
		/// <returns></returns>
		static double ReturnDouble()
		{
			return 2.71828;
		}

		[TestMethod]
		public void TestSimpleReturn()
		{
			Assert.AreEqual(ReturnSmallInt(), Run<int>(nameof(ReturnSmallInt)));
			Assert.AreEqual(ReturnLargeInt(), Run<int>(nameof(ReturnLargeInt)));
			Assert.AreEqual(ReturnFloat(), Run<float>(nameof(ReturnFloat)));
			Assert.AreEqual(ReturnDouble(), Run<double>(nameof(ReturnDouble)));
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
			Run<int>(nameof(EmptyMethod));
		}
	}
}
