﻿using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KWeb.Server
{
    public class MimeHelper
    {
        // See more
        // https://referencesource.microsoft.com/#system.web/MimeMapping.cs,101
        public static string GetContentType(string extension)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;
            if (_dic.ContainsKey(extension))
                return _dic[extension];
            return "application/octet-stream";
        }

        private static readonly ConcurrentDictionary<string, string> _dic = new ConcurrentDictionary<string, string>(
            new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(".323", "text/h323"),
                new KeyValuePair<string, string>(".aaf", "application/octet-stream"),
                new KeyValuePair<string, string>(".aca", "application/octet-stream"),
                new KeyValuePair<string, string>(".accdb", "application/msaccess"),
                new KeyValuePair<string, string>(".accde", "application/msaccess"),
                new KeyValuePair<string, string>(".accdt", "application/msaccess"),
                new KeyValuePair<string, string>(".acx", "application/internet-property-stream"),
                new KeyValuePair<string, string>(".afm", "application/octet-stream"),
                new KeyValuePair<string, string>(".ai", "application/postscript"),
                new KeyValuePair<string, string>(".aif", "audio/x-aiff"),
                new KeyValuePair<string, string>(".aifc", "audio/aiff"),
                new KeyValuePair<string, string>(".aiff", "audio/aiff"),
                new KeyValuePair<string, string>(".application", "application/x-ms-application"),
                new KeyValuePair<string, string>(".art", "image/x-jg"),
                new KeyValuePair<string, string>(".asd", "application/octet-stream"),
                new KeyValuePair<string, string>(".asf", "video/x-ms-asf"),
                new KeyValuePair<string, string>(".asi", "application/octet-stream"),
                new KeyValuePair<string, string>(".asm", "text/plain"),
                new KeyValuePair<string, string>(".asr", "video/x-ms-asf"),
                new KeyValuePair<string, string>(".asx", "video/x-ms-asf"),
                new KeyValuePair<string, string>(".atom", "application/atom+xml"),
                new KeyValuePair<string, string>(".au", "audio/basic"),
                new KeyValuePair<string, string>(".avi", "video/x-msvideo"),
                new KeyValuePair<string, string>(".axs", "application/olescript"),
                new KeyValuePair<string, string>(".bas", "text/plain"),
                new KeyValuePair<string, string>(".bcpio", "application/x-bcpio"),
                new KeyValuePair<string, string>(".bin", "application/octet-stream"),
                new KeyValuePair<string, string>(".bmp", "image/bmp"),
                new KeyValuePair<string, string>(".c", "text/plain"),
                new KeyValuePair<string, string>(".cab", "application/octet-stream"),
                new KeyValuePair<string, string>(".calx", "application/vnd.ms-office.calx"),
                new KeyValuePair<string, string>(".cat", "application/vnd.ms-pki.seccat"),
                new KeyValuePair<string, string>(".cdf", "application/x-cdf"),
                new KeyValuePair<string, string>(".chm", "application/octet-stream"),
                new KeyValuePair<string, string>(".class", "application/x-java-applet"),
                new KeyValuePair<string, string>(".clp", "application/x-msclip"),
                new KeyValuePair<string, string>(".cmx", "image/x-cmx"),
                new KeyValuePair<string, string>(".cnf", "text/plain"),
                new KeyValuePair<string, string>(".cod", "image/cis-cod"),
                new KeyValuePair<string, string>(".cpio", "application/x-cpio"),
                new KeyValuePair<string, string>(".cpp", "text/plain"),
                new KeyValuePair<string, string>(".crd", "application/x-mscardfile"),
                new KeyValuePair<string, string>(".crl", "application/pkix-crl"),
                new KeyValuePair<string, string>(".crt", "application/x-x509-ca-cert"),
                new KeyValuePair<string, string>(".csh", "application/x-csh"),
                new KeyValuePair<string, string>(".css", "text/css"),
                new KeyValuePair<string, string>(".csv", "application/octet-stream"),
                new KeyValuePair<string, string>(".cur", "application/octet-stream"),
                new KeyValuePair<string, string>(".dcr", "application/x-director"),
                new KeyValuePair<string, string>(".deploy", "application/octet-stream"),
                new KeyValuePair<string, string>(".der", "application/x-x509-ca-cert"),
                new KeyValuePair<string, string>(".dib", "image/bmp"),
                new KeyValuePair<string, string>(".dir", "application/x-director"),
                new KeyValuePair<string, string>(".disco", "text/xml"),
                new KeyValuePair<string, string>(".dll", "application/x-msdownload"),
                new KeyValuePair<string, string>(".dll.config", "text/xml"),
                new KeyValuePair<string, string>(".dlm", "text/dlm"),
                new KeyValuePair<string, string>(".doc", "application/msword"),
                new KeyValuePair<string, string>(".docm", "application/vnd.ms-word.document.macroEnabled.12"),
                new KeyValuePair<string, string>(".docx",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"),
                new KeyValuePair<string, string>(".dot", "application/msword"),
                new KeyValuePair<string, string>(".dotm", "application/vnd.ms-word.template.macroEnabled.12"),
                new KeyValuePair<string, string>(".dotx",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.template"),
                new KeyValuePair<string, string>(".dsp", "application/octet-stream"),
                new KeyValuePair<string, string>(".dtd", "text/xml"),
                new KeyValuePair<string, string>(".dvi", "application/x-dvi"),
                new KeyValuePair<string, string>(".dwf", "drawing/x-dwf"),
                new KeyValuePair<string, string>(".dwp", "application/octet-stream"),
                new KeyValuePair<string, string>(".dxr", "application/x-director"),
                new KeyValuePair<string, string>(".eml", "message/rfc822"),
                new KeyValuePair<string, string>(".emz", "application/octet-stream"),
                new KeyValuePair<string, string>(".eot", "application/octet-stream"),
                new KeyValuePair<string, string>(".eps", "application/postscript"),
                new KeyValuePair<string, string>(".etx", "text/x-setext"),
                new KeyValuePair<string, string>(".evy", "application/envoy"),
                new KeyValuePair<string, string>(".exe", "application/octet-stream"),
                new KeyValuePair<string, string>(".exe.config", "text/xml"),
                new KeyValuePair<string, string>(".fdf", "application/vnd.fdf"),
                new KeyValuePair<string, string>(".fif", "application/fractals"),
                new KeyValuePair<string, string>(".fla", "application/octet-stream"),
                new KeyValuePair<string, string>(".flr", "x-world/x-vrml"),
                new KeyValuePair<string, string>(".flv", "video/x-flv"),
                new KeyValuePair<string, string>(".gif", "image/gif"),
                new KeyValuePair<string, string>(".gtar", "application/x-gtar"),
                new KeyValuePair<string, string>(".gz", "application/x-gzip"),
                new KeyValuePair<string, string>(".h", "text/plain"),
                new KeyValuePair<string, string>(".hdf", "application/x-hdf"),
                new KeyValuePair<string, string>(".hdml", "text/x-hdml"),
                new KeyValuePair<string, string>(".hhc", "application/x-oleobject"),
                new KeyValuePair<string, string>(".hhk", "application/octet-stream"),
                new KeyValuePair<string, string>(".hhp", "application/octet-stream"),
                new KeyValuePair<string, string>(".hlp", "application/winhlp"),
                new KeyValuePair<string, string>(".hqx", "application/mac-binhex40"),
                new KeyValuePair<string, string>(".hta", "application/hta"),
                new KeyValuePair<string, string>(".htc", "text/x-component"),
                new KeyValuePair<string, string>(".htm", "text/html"),
                new KeyValuePair<string, string>(".html", "text/html"),
                new KeyValuePair<string, string>(".htt", "text/webviewhtml"),
                new KeyValuePair<string, string>(".hxt", "text/html"),
                new KeyValuePair<string, string>(".ico", "image/x-icon"),
                new KeyValuePair<string, string>(".ics", "application/octet-stream"),
                new KeyValuePair<string, string>(".ief", "image/ief"),
                new KeyValuePair<string, string>(".iii", "application/x-iphone"),
                new KeyValuePair<string, string>(".inf", "application/octet-stream"),
                new KeyValuePair<string, string>(".ins", "application/x-internet-signup"),
                new KeyValuePair<string, string>(".isp", "application/x-internet-signup"),
                new KeyValuePair<string, string>(".IVF", "video/x-ivf"),
                new KeyValuePair<string, string>(".jar", "application/java-archive"),
                new KeyValuePair<string, string>(".java", "application/octet-stream"),
                new KeyValuePair<string, string>(".jck", "application/liquidmotion"),
                new KeyValuePair<string, string>(".jcz", "application/liquidmotion"),
                new KeyValuePair<string, string>(".jfif", "image/pjpeg"),
                new KeyValuePair<string, string>(".jpb", "application/octet-stream"),
                new KeyValuePair<string, string>(".jpe", "image/jpeg"),
                new KeyValuePair<string, string>(".jpeg", "image/jpeg"),
                new KeyValuePair<string, string>(".jpg", "image/jpeg"),
                new KeyValuePair<string, string>(".js", "application/x-javascript"),
                new KeyValuePair<string, string>(".jsx", "text/jscript"),
                new KeyValuePair<string, string>(".latex", "application/x-latex"),
                new KeyValuePair<string, string>(".lit", "application/x-ms-reader"),
                new KeyValuePair<string, string>(".lpk", "application/octet-stream"),
                new KeyValuePair<string, string>(".lsf", "video/x-la-asf"),
                new KeyValuePair<string, string>(".lsx", "video/x-la-asf"),
                new KeyValuePair<string, string>(".lzh", "application/octet-stream"),
                new KeyValuePair<string, string>(".m13", "application/x-msmediaview"),
                new KeyValuePair<string, string>(".m14", "application/x-msmediaview"),
                new KeyValuePair<string, string>(".m1v", "video/mpeg"),
                new KeyValuePair<string, string>(".m3u", "audio/x-mpegurl"),
                new KeyValuePair<string, string>(".man", "application/x-troff-man"),
                new KeyValuePair<string, string>(".manifest", "application/x-ms-manifest"),
                new KeyValuePair<string, string>(".map", "text/plain"),
                new KeyValuePair<string, string>(".mdb", "application/x-msaccess"),
                new KeyValuePair<string, string>(".mdp", "application/octet-stream"),
                new KeyValuePair<string, string>(".me", "application/x-troff-me"),
                new KeyValuePair<string, string>(".mht", "message/rfc822"),
                new KeyValuePair<string, string>(".mhtml", "message/rfc822"),
                new KeyValuePair<string, string>(".mid", "audio/mid"),
                new KeyValuePair<string, string>(".midi", "audio/mid"),
                new KeyValuePair<string, string>(".mix", "application/octet-stream"),
                new KeyValuePair<string, string>(".mmf", "application/x-smaf"),
                new KeyValuePair<string, string>(".mno", "text/xml"),
                new KeyValuePair<string, string>(".mny", "application/x-msmoney"),
                new KeyValuePair<string, string>(".mov", "video/quicktime"),
                new KeyValuePair<string, string>(".movie", "video/x-sgi-movie"),
                new KeyValuePair<string, string>(".mp2", "video/mpeg"),
                new KeyValuePair<string, string>(".mp3", "audio/mpeg"),
                new KeyValuePair<string, string>(".mpa", "video/mpeg"),
                new KeyValuePair<string, string>(".mpe", "video/mpeg"),
                new KeyValuePair<string, string>(".mpeg", "video/mpeg"),
                new KeyValuePair<string, string>(".mpg", "video/mpeg"),
                new KeyValuePair<string, string>(".mpp", "application/vnd.ms-project"),
                new KeyValuePair<string, string>(".mpv2", "video/mpeg"),
                new KeyValuePair<string, string>(".ms", "application/x-troff-ms"),
                new KeyValuePair<string, string>(".msi", "application/octet-stream"),
                new KeyValuePair<string, string>(".mso", "application/octet-stream"),
                new KeyValuePair<string, string>(".mvb", "application/x-msmediaview"),
                new KeyValuePair<string, string>(".mvc", "application/x-miva-compiled"),
                new KeyValuePair<string, string>(".nc", "application/x-netcdf"),
                new KeyValuePair<string, string>(".nsc", "video/x-ms-asf"),
                new KeyValuePair<string, string>(".nws", "message/rfc822"),
                new KeyValuePair<string, string>(".ocx", "application/octet-stream"),
                new KeyValuePair<string, string>(".oda", "application/oda"),
                new KeyValuePair<string, string>(".odc", "text/x-ms-odc"),
                new KeyValuePair<string, string>(".ods", "application/oleobject"),
                new KeyValuePair<string, string>(".one", "application/onenote"),
                new KeyValuePair<string, string>(".onea", "application/onenote"),
                new KeyValuePair<string, string>(".onetoc", "application/onenote"),
                new KeyValuePair<string, string>(".onetoc2", "application/onenote"),
                new KeyValuePair<string, string>(".onetmp", "application/onenote"),
                new KeyValuePair<string, string>(".onepkg", "application/onenote"),
                new KeyValuePair<string, string>(".osdx", "application/opensearchdescription+xml"),
                new KeyValuePair<string, string>(".p10", "application/pkcs10"),
                new KeyValuePair<string, string>(".p12", "application/x-pkcs12"),
                new KeyValuePair<string, string>(".p7b", "application/x-pkcs7-certificates"),
                new KeyValuePair<string, string>(".p7c", "application/pkcs7-mime"),
                new KeyValuePair<string, string>(".p7m", "application/pkcs7-mime"),
                new KeyValuePair<string, string>(".p7r", "application/x-pkcs7-certreqresp"),
                new KeyValuePair<string, string>(".p7s", "application/pkcs7-signature"),
                new KeyValuePair<string, string>(".pbm", "image/x-portable-bitmap"),
                new KeyValuePair<string, string>(".pcx", "application/octet-stream"),
                new KeyValuePair<string, string>(".pcz", "application/octet-stream"),
                new KeyValuePair<string, string>(".pdf", "application/pdf"),
                new KeyValuePair<string, string>(".pfb", "application/octet-stream"),
                new KeyValuePair<string, string>(".pfm", "application/octet-stream"),
                new KeyValuePair<string, string>(".pfx", "application/x-pkcs12"),
                new KeyValuePair<string, string>(".pgm", "image/x-portable-graymap"),
                new KeyValuePair<string, string>(".pko", "application/vnd.ms-pki.pko"),
                new KeyValuePair<string, string>(".pma", "application/x-perfmon"),
                new KeyValuePair<string, string>(".pmc", "application/x-perfmon"),
                new KeyValuePair<string, string>(".pml", "application/x-perfmon"),
                new KeyValuePair<string, string>(".pmr", "application/x-perfmon"),
                new KeyValuePair<string, string>(".pmw", "application/x-perfmon"),
                new KeyValuePair<string, string>(".png", "image/png"),
                new KeyValuePair<string, string>(".pnm", "image/x-portable-anymap"),
                new KeyValuePair<string, string>(".pnz", "image/png"),
                new KeyValuePair<string, string>(".pot", "application/vnd.ms-powerpoint"),
                new KeyValuePair<string, string>(".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"),
                new KeyValuePair<string, string>(".potx",
                    "application/vnd.openxmlformats-officedocument.presentationml.template"),
                new KeyValuePair<string, string>(".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"),
                new KeyValuePair<string, string>(".ppm", "image/x-portable-pixmap"),
                new KeyValuePair<string, string>(".pps", "application/vnd.ms-powerpoint"),
                new KeyValuePair<string, string>(".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"),
                new KeyValuePair<string, string>(".ppsx",
                    "application/vnd.openxmlformats-officedocument.presentationml.slideshow"),
                new KeyValuePair<string, string>(".ppt", "application/vnd.ms-powerpoint"),
                new KeyValuePair<string, string>(".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"),
                new KeyValuePair<string, string>(".pptx",
                    "application/vnd.openxmlformats-officedocument.presentationml.presentation"),
                new KeyValuePair<string, string>(".prf", "application/pics-rules"),
                new KeyValuePair<string, string>(".prm", "application/octet-stream"),
                new KeyValuePair<string, string>(".prx", "application/octet-stream"),
                new KeyValuePair<string, string>(".ps", "application/postscript"),
                new KeyValuePair<string, string>(".psd", "application/octet-stream"),
                new KeyValuePair<string, string>(".psm", "application/octet-stream"),
                new KeyValuePair<string, string>(".psp", "application/octet-stream"),
                new KeyValuePair<string, string>(".pub", "application/x-mspublisher"),
                new KeyValuePair<string, string>(".qt", "video/quicktime"),
                new KeyValuePair<string, string>(".qtl", "application/x-quicktimeplayer"),
                new KeyValuePair<string, string>(".qxd", "application/octet-stream"),
                new KeyValuePair<string, string>(".ra", "audio/x-pn-realaudio"),
                new KeyValuePair<string, string>(".ram", "audio/x-pn-realaudio"),
                new KeyValuePair<string, string>(".rar", "application/octet-stream"),
                new KeyValuePair<string, string>(".ras", "image/x-cmu-raster"),
                new KeyValuePair<string, string>(".rf", "image/vnd.rn-realflash"),
                new KeyValuePair<string, string>(".rgb", "image/x-rgb"),
                new KeyValuePair<string, string>(".rm", "application/vnd.rn-realmedia"),
                new KeyValuePair<string, string>(".rmi", "audio/mid"),
                new KeyValuePair<string, string>(".roff", "application/x-troff"),
                new KeyValuePair<string, string>(".rpm", "audio/x-pn-realaudio-plugin"),
                new KeyValuePair<string, string>(".rtf", "application/rtf"),
                new KeyValuePair<string, string>(".rtx", "text/richtext"),
                new KeyValuePair<string, string>(".scd", "application/x-msschedule"),
                new KeyValuePair<string, string>(".sct", "text/scriptlet"),
                new KeyValuePair<string, string>(".sea", "application/octet-stream"),
                new KeyValuePair<string, string>(".setpay", "application/set-payment-initiation"),
                new KeyValuePair<string, string>(".setreg", "application/set-registration-initiation"),
                new KeyValuePair<string, string>(".sgml", "text/sgml"),
                new KeyValuePair<string, string>(".sh", "application/x-sh"),
                new KeyValuePair<string, string>(".shar", "application/x-shar"),
                new KeyValuePair<string, string>(".sit", "application/x-stuffit"),
                new KeyValuePair<string, string>(".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"),
                new KeyValuePair<string, string>(".sldx",
                    "application/vnd.openxmlformats-officedocument.presentationml.slide"),
                new KeyValuePair<string, string>(".smd", "audio/x-smd"),
                new KeyValuePair<string, string>(".smi", "application/octet-stream"),
                new KeyValuePair<string, string>(".smx", "audio/x-smd"),
                new KeyValuePair<string, string>(".smz", "audio/x-smd"),
                new KeyValuePair<string, string>(".snd", "audio/basic"),
                new KeyValuePair<string, string>(".snp", "application/octet-stream"),
                new KeyValuePair<string, string>(".spc", "application/x-pkcs7-certificates"),
                new KeyValuePair<string, string>(".spl", "application/futuresplash"),
                new KeyValuePair<string, string>(".src", "application/x-wais-source"),
                new KeyValuePair<string, string>(".ssm", "application/streamingmedia"),
                new KeyValuePair<string, string>(".sst", "application/vnd.ms-pki.certstore"),
                new KeyValuePair<string, string>(".stl", "application/vnd.ms-pki.stl"),
                new KeyValuePair<string, string>(".sv4cpio", "application/x-sv4cpio"),
                new KeyValuePair<string, string>(".sv4crc", "application/x-sv4crc"),
                new KeyValuePair<string, string>(".swf", "application/x-shockwave-flash"),
                new KeyValuePair<string, string>(".t", "application/x-troff"),
                new KeyValuePair<string, string>(".tar", "application/x-tar"),
                new KeyValuePair<string, string>(".tcl", "application/x-tcl"),
                new KeyValuePair<string, string>(".tex", "application/x-tex"),
                new KeyValuePair<string, string>(".texi", "application/x-texinfo"),
                new KeyValuePair<string, string>(".texinfo", "application/x-texinfo"),
                new KeyValuePair<string, string>(".tgz", "application/x-compressed"),
                new KeyValuePair<string, string>(".thmx", "application/vnd.ms-officetheme"),
                new KeyValuePair<string, string>(".thn", "application/octet-stream"),
                new KeyValuePair<string, string>(".tif", "image/tiff"),
                new KeyValuePair<string, string>(".tiff", "image/tiff"),
                new KeyValuePair<string, string>(".toc", "application/octet-stream"),
                new KeyValuePair<string, string>(".tr", "application/x-troff"),
                new KeyValuePair<string, string>(".trm", "application/x-msterminal"),
                new KeyValuePair<string, string>(".tsv", "text/tab-separated-values"),
                new KeyValuePair<string, string>(".ttf", "application/octet-stream"),
                new KeyValuePair<string, string>(".txt", "text/plain"),
                new KeyValuePair<string, string>(".u32", "application/octet-stream"),
                new KeyValuePair<string, string>(".uls", "text/iuls"),
                new KeyValuePair<string, string>(".ustar", "application/x-ustar"),
                new KeyValuePair<string, string>(".vbs", "text/vbscript"),
                new KeyValuePair<string, string>(".vcf", "text/x-vcard"),
                new KeyValuePair<string, string>(".vcs", "text/plain"),
                new KeyValuePair<string, string>(".vdx", "application/vnd.ms-visio.viewer"),
                new KeyValuePair<string, string>(".vml", "text/xml"),
                new KeyValuePair<string, string>(".vsd", "application/vnd.visio"),
                new KeyValuePair<string, string>(".vss", "application/vnd.visio"),
                new KeyValuePair<string, string>(".vst", "application/vnd.visio"),
                new KeyValuePair<string, string>(".vsto", "application/x-ms-vsto"),
                new KeyValuePair<string, string>(".vsw", "application/vnd.visio"),
                new KeyValuePair<string, string>(".vsx", "application/vnd.visio"),
                new KeyValuePair<string, string>(".vtx", "application/vnd.visio"),
                new KeyValuePair<string, string>(".wav", "audio/wav"),
                new KeyValuePair<string, string>(".wax", "audio/x-ms-wax"),
                new KeyValuePair<string, string>(".wbmp", "image/vnd.wap.wbmp"),
                new KeyValuePair<string, string>(".wcm", "application/vnd.ms-works"),
                new KeyValuePair<string, string>(".wdb", "application/vnd.ms-works"),
                new KeyValuePair<string, string>(".wks", "application/vnd.ms-works"),
                new KeyValuePair<string, string>(".wm", "video/x-ms-wm"),
                new KeyValuePair<string, string>(".wma", "audio/x-ms-wma"),
                new KeyValuePair<string, string>(".wmd", "application/x-ms-wmd"),
                new KeyValuePair<string, string>(".wmf", "application/x-msmetafile"),
                new KeyValuePair<string, string>(".wml", "text/vnd.wap.wml"),
                new KeyValuePair<string, string>(".wmlc", "application/vnd.wap.wmlc"),
                new KeyValuePair<string, string>(".wmls", "text/vnd.wap.wmlscript"),
                new KeyValuePair<string, string>(".wmlsc", "application/vnd.wap.wmlscriptc"),
                new KeyValuePair<string, string>(".wmp", "video/x-ms-wmp"),
                new KeyValuePair<string, string>(".wmv", "video/x-ms-wmv"),
                new KeyValuePair<string, string>(".wmx", "video/x-ms-wmx"),
                new KeyValuePair<string, string>(".wmz", "application/x-ms-wmz"),
                new KeyValuePair<string, string>(".wps", "application/vnd.ms-works"),
                new KeyValuePair<string, string>(".wri", "application/x-mswrite"),
                new KeyValuePair<string, string>(".wrl", "x-world/x-vrml"),
                new KeyValuePair<string, string>(".wrz", "x-world/x-vrml"),
                new KeyValuePair<string, string>(".wsdl", "text/xml"),
                new KeyValuePair<string, string>(".wvx", "video/x-ms-wvx"),
                new KeyValuePair<string, string>(".x", "application/directx"),
                new KeyValuePair<string, string>(".xaf", "x-world/x-vrml"),
                new KeyValuePair<string, string>(".xaml", "application/xaml+xml"),
                new KeyValuePair<string, string>(".xap", "application/x-silverlight-app"),
                new KeyValuePair<string, string>(".xbap", "application/x-ms-xbap"),
                new KeyValuePair<string, string>(".xbm", "image/x-xbitmap"),
                new KeyValuePair<string, string>(".xdr", "text/plain"),
                new KeyValuePair<string, string>(".xla", "application/vnd.ms-excel"),
                new KeyValuePair<string, string>(".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"),
                new KeyValuePair<string, string>(".xlc", "application/vnd.ms-excel"),
                new KeyValuePair<string, string>(".xlm", "application/vnd.ms-excel"),
                new KeyValuePair<string, string>(".xls", "application/vnd.ms-excel"),
                new KeyValuePair<string, string>(".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"),
                new KeyValuePair<string, string>(".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"),
                new KeyValuePair<string, string>(".xlsx",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"),
                new KeyValuePair<string, string>(".xlt", "application/vnd.ms-excel"),
                new KeyValuePair<string, string>(".xltm", "application/vnd.ms-excel.template.macroEnabled.12"),
                new KeyValuePair<string, string>(".xltx",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.template"),
                new KeyValuePair<string, string>(".xlw", "application/vnd.ms-excel"),
                new KeyValuePair<string, string>(".xml", "text/xml"),
                new KeyValuePair<string, string>(".xof", "x-world/x-vrml"),
                new KeyValuePair<string, string>(".xpm", "image/x-xpixmap"),
                new KeyValuePair<string, string>(".xps", "application/vnd.ms-xpsdocument"),
                new KeyValuePair<string, string>(".xsd", "text/xml"),
                new KeyValuePair<string, string>(".xsf", "text/xml"),
                new KeyValuePair<string, string>(".xsl", "text/xml"),
                new KeyValuePair<string, string>(".xslt", "text/xml"),
                new KeyValuePair<string, string>(".xsn", "application/octet-stream"),
                new KeyValuePair<string, string>(".xtp", "application/octet-stream"),
                new KeyValuePair<string, string>(".xwd", "image/x-xwindowdump"),
                new KeyValuePair<string, string>(".z", "application/x-compress"),
                new KeyValuePair<string, string>(".zip", "application/x-zip-compressed"),
            });
    }
}