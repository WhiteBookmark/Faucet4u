import Vue from "vue";
import Vuex from "vuex";
import moment from "moment";
import pathify from '@/scripts/pathify';
import { make } from 'vuex-pathify';

Vue.use(Vuex);

const stateData = {
    sessionValid: false,
    authorization: 0,
    referrer: null,
    viewTicketsReference: null,
    userData: [],
    loginHistory: null,
    withdrawalHistory: null,
    level1Referrals: null,
    level2Referrals: null,
    level3Referrals: null,
    faucetSettings: [{ "Name": "BonusAdCredit", "Value": "0.00000100" }, { "Name": "BonusAdsCredit", "Value": "0.00000100" }, { "Name": "CheatCounterForDirectClaims", "Value": "1" }, { "Name": "CheatCounterForDuplicateAccount", "Value": "1" }, { "Name": "CheatCounterForMultipleAccounts", "Value": "5" }, { "Name": "CheatCounterForProxy", "Value": "5" }, { "Name": "DirectNonUniqueIP1Credit", "Value": "0.00010000" }, { "Name": "DirectNonUniqueIP2Credit", "Value": "0.00010000" }, { "Name": "DirectNonUniqueIP3Credit", "Value": "0.00001000" }, { "Name": "DirectUniqueIPCredit", "Value": "0.00001000" }, { "Name": "ForumLink", "Value": "http://www.ems.com" }, { "Name": "HitsFor1BonusAdDayCredit", "Value": "10000" }, { "Name": "HitsFor1PTPDayCredit", "Value": "10000" }, { "Name": "ImpressionsFor1BannerDayCredit", "Value": "10000" }, { "Name": "ImpressionsFor1SquareBannerDayCredit", "Value": "10000" }, { "Name": "Level1", "Value": "10" }, { "Name": "Level2", "Value": "5" }, { "Name": "Level3", "Value": "2" }, { "Name": "LinkClaimCredit", "Value": "0.00010000" }, { "Name": "LinkClaimMinutes", "Value": "3" }, { "Name": "MinimumDeposit", "Value": "0.00010000" }, { "Name": "MinimumExchangeWithdrawal", "Value": "0.00150000" }, { "Name": "MinimumOfferwallWithdrawal", "Value": "0.00030000" }, { "Name": "MinimumWithdrawal", "Value": "0.00020000" }, { "Name": "OfferwallLevel1", "Value": "10" }, { "Name": "OfferwallLevel2", "Value": "5" }, { "Name": "OfferwallLevel3", "Value": "2" }, { "Name": "PTPDirectRatio", "Value": "0" }, { "Name": "PTPLevel1", "Value": "10" }, { "Name": "PTPLevel2", "Value": "5" }, { "Name": "PTPLevel3", "Value": "2" }, { "Name": "PTPNonUniqueIP1Credit", "Value": "0.00010000" }, { "Name": "PTPNonUniqueIP2Credit", "Value": "0.00010000" }, { "Name": "PTPNonUniqueIP3Credit", "Value": "0.00001000" }, { "Name": "PTPTotalRatio", "Value": "5" }, { "Name": "PTPUniqueIPCredit", "Value": "0.00001000" }],
    miningPercentage: null,
    advertisePTP: null,
    advertisePTPTimeBased: null,
    advertiseBanner: null,
    advertiseBannerTimeBased: null,
    purchaseOrder: null,
    purchaseOrderType: null,
    orderHistory: null,
    depositHistory: null,
    isLoading: false,
    inappropriateWords:
        "2g1c|a55|a55hole|acrotomophilia|aeolus|ahole|alabama hot pocket|alaskan pipeline|anal|analprobe|anilingus|anus|apeshit|areola|areole|arian|arsehole|aryan|ass|ass hole|assbang|assbanged|assbangs|asses|assfuck|assfucker|assh0le|asshat|assho1e|asshole|assholes|assmaster|assmunch|asswipe|asswipes|auto erotic|autoerotic|azazel|azz|b1tch|babe|babeland|babes|baby batter|baby juice|ball gag|ball gravy|ball kicking|ball licking|ball sack|ball sucking|ballsack|bang|bangbros|banger|bareback|barely legal|barenaked|barf|bastard|bastardo|bastards|bastinado|bawdy|bbw|bdsm|beaner|beaners|beardedclam|beastiality|beatch|beater|beaver|beaver cleaver|beaver lips|beer|beeyotch|beotch|bestiality|biatch|big black|big breasts|big knockers|big tits|bigtits|bimbo|bimbos|birdlock|bitch|bitched|bitches|bitchy|black cock|blonde action|blonde on blonde action|blow|blow job|blow your load|blowjob|blowjobs|blue waffle|blumpkin|bod|bodily|boink|bollock|bollocks|bollok|bondage|bone|boned|boner|boners|bong|boob|boobies|boobs|booby|booger|bookie|bootee|bootie|booty|booty call|booze|boozer|boozy|bosom|bosomy|bowel|bowels|bra|brassiere|breast|breasts|brown showers|brunette action|bugger|bukkake|bull shit|bulldyke|bullet vibe|bullshit|bullshits|bullshitted|bullturds|bung|bung hole|bunghole|busty|butt|butt fuck|buttcheeks|buttfuck|buttfucker|butthole|buttplug|c0ck|c-0-c-k|c-o-c-k|c-u-n-t|c.0.c.k|c.o.c.k.|c.u.n.t|caca|cahone|camel toe|cameltoe|camgirl|camslut|camwhore|carpet muncher|carpetmuncher|cawk|cervix|chinc|chincs|chink|chocolate rosebuds|chode|chodes|circlejerk|cl1t|cleveland steamer|climax|clit|clitoris|clitorus|clits|clitty|clover clamps|clusterfuck|cocain|cocaine|cock|cock sucker|cockblock|cockholster|cockknocker|cocks|cocksmoker|cocksucker|coital|commie|condom|coon|coons|coprolagnia|coprophilia|corksucker|cornhole|crabs|crack|cracker|crackwhore|crap|crappy|creampie|cum|cummin|cumming|cumshot|cumshots|cumslut|cumstain|cunilingus|cunnilingus|cunny|cunt|cuntface|cunthunter|cuntlick|cuntlicker|cunts|d0ng|d0uch3|d0uche|d1ck|d1ld0|d1ldo|dafuq|dago|dagos|dammit|damn|damned|damnit|darkie|date rape|daterape|dawgie-style|deep throat|deepthroat|dendrophilia|dick|dick-ish|dickbag|dickdipper|dickface|dickflipper|dickhead|dickheads|dickish|dickripper|dicksipper|dickweed|dickwhipper|dickzipper|diddle|dike|dildo|dildos|diligaf|dillweed|dimwit|dingle|dingleberries|dingleberry|dipship|dirty pillows|dirty sanchez|dog style|doggie style|doggie-style|doggiestyle|doggy style|doggy-style|doggystyle|dolcett|domination|dominatrix|dommes|dong|donkey punch|doofus|doosh|dopey|double dong|double penetration|douch3|douche|douchebag|douchebags|douchey|dp action|drunk|dry hump|dumass|dumbass|dumbasses|dummy|dvda|dyke|dykes|eat my ass|ecchi|ejaculate|ejaculation|enlargement|erect|erection|erotic|erotism|escort|essohbee|eunuch|extacy|extasy|f-u-c-k|f.u.c.k|fack|fag|fagg|fagged|faggit|faggot|fagot|fags|faig|faigt|fannybandit|fart|fartknocker|fat|fecal|felch|felcher|felching|fellate|fellatio|feltch|feltcher|female squirting|femdom|figging|fingerbang|fingering|fisted|fisting|fisty|floozy|foad|fondle|foobar|foot fetish|footjob|foreskin|freex|frigg|frigga|frotting|fubar|fuck|fuck buttons|fuck-tard|fuckass|fucked|fucker|fuckface|fuckin|fucking|fucknugget|fucknut|fuckoff|fucks|fucktard|fucktards|fuckup|fuckwad|fuckwit|fudge packer|fudgepacker|fuk|futanari|fvck|fxck|g-spot|gae|gai|gang bang|ganja|gay|gay sex|gays|genitals|gey|gfy|ghay|ghey|giant cock|gigolo|girl on|girl on top|girls gone wild|glans|goatcx|goatse|god damn|godamn|godamnit|goddam|goddammit|goddamn|gokkun|golden shower|goldenshower|gonad|gonads|goo girl|goodpoop|gook|gooks|goregasm|gringo|grope|group sex|gspot|gtfo|guido|guro|h0m0|h0mo|hand job|handjob|hard core|hard on|hardcore|he11|hebe|heeb|hell|hemp|hentai|heroin|herp|herpes|herpy|hitler|hiv|hobag|hom0|homey|homo|homoerotic|homoey|honkey|honky|hooch|hookah|hooker|hoor|hootch|hooter|hooters|horny|hot carl|hot chick|how to kill|how to murder|huge fat|hump|humped|humping|hussy|hymen|inbred|incest|injun|intercourse|j3rk0ff|jack off|jackass|jackhole|jackoff|jail bait|jailbait|jap|japs|jelly donut|jerk|jerk0ff|jerk off|jerked|jerkoff|jigaboo|jiggaboo|jiggerboo|jism|jiz|jizm|jizz|jizzed|juggs|junkie|junky|kike|kikes|kill|kinbaku|kinkster|kinky|kkk|klan|knobbing|knobend|kooch|kooches|kootch|kraut|kyke|labia|leather restraint|leather straight jacket|lech|lemon party|leper|lesbian|lesbians|lesbo|lesbos|lez|lezbian|lezbians|lezbo|lezbos|lezzie|lezzies|lezzy|lmao|lmfao|loin|loins|lolita|lovemaking|lube|lusty|m-fucking|make me come|male squirting|mams|massa|masterbate|masterbating|masterbation|masturbate|masturbating|masturbation|maxi|menage a trois|menses|menstruate|menstruation|meth|milf|missionary position|mofo|molest|moolie|moron|motherfucka|motherfucker|motherfucking|mound of venus|mr hands|mtherfucker|mthrfucker|mthrfucking|muff|muff diver|muffdiver|muffdiving|murder|muthafuckaz|muthafucker|mutherfucker|mutherfucking|muthrfucking|nad|nads|naked|nambla|napalm|nappy|nawashi|nazi|nazism|negro|neonazi|nig nog|nigga|niggah|niggas|niggaz|nigger|niggers|niggle|niglet|nimphomania|nimrod|ninny|nipple|nipples|nooky|nsfw images|nude|nudity|nympho|nymphomania|octopussy|omorashi|one cup two girls|one guy one jar|opiate|opium|oral|orally|organ|orgasm|orgasmic|orgies|orgy|ovary|ovum|ovums|p.u.s.s.y.|paddy|paedophile|paki|pantie|panties|panty|pastie|pasty|pcp|pecker|pedo|pedobear|pedophile|pedophilia|pedophiliac|pee|peepee|pegging|penetrate|penetration|penial|penile|penis|perversion|peyote|phalli|phallic|phone sex|phuck|piece of shit|pillowbiter|pimp|pinko|piss|piss pig|piss-off|pissed|pissing|pissoff|pisspig|playboy|pleasure chest|pms|polack|pole smoker|pollock|ponyplay|poof|poon|poontang|poop chute|poopchute|porn|porno|pornography|pot|potty|prick|prig|prince albert piercing|prostitute|prude|pthc|pube|pubes|pubic|pubis|punany|punkass|punky|puss|pussies|pussy|pussypounder|puto|queaf|queef|queer|queero|queers|quicky|quim|r-tard|racy|raghead|raging boner|rape|raped|raper|raping|rapist|raunch|rectal|rectum|rectus|reefer|reetard|reich|retard|retarded|reverse cowgirl|revue|rimjob|rimming|ritard|rosy palm|rosy palm and her 5 sisters|rtard|rum|rump|rumprammer|ruski|rusty trombone|s0b|s&m|s-h-1-t|s-h-i-t|s-o-b|s.h.i.t.|s.o.b.|sadism|sadist|santorum|scag|scantily|scat|schizo|schlong|scissoring|screw|screwed|scrog|scrot|scrote|scrotum|scrud|scum|seaman|seamen|seduce|semen|sex|sexo|sexual|sexy|sh1t|shamedame|shaved beaver|shaved pussy|shemale|shibari|shit|shitblimp|shite|shiteater|shitface|shithead|shithole|shithouse|shits|shitt|shitted|shitter|shitty|shiz|shota|shrimping|sissy|skag|skank|skeet|slanteye|slave|sleaze|sleazy|slut|slutdumper|slutkiss|sluts|smegma|smut|smutty|snatch|sniper|snowballing|snuff|sodom|sodomize|sodomy|souse|soused|sperm|spic|spick|spik|spiks|splooge|splooge moose|spooge|spread legs|spunk|steamy|stfu|stiffy|stoned|strap on|strapon|strappado|strip|strip club|stroke|stupid|style doggy|suck|sucked|sucking|sucks|suicide girls|sultry women|sumofabiatch|swastika|swinger|t1t|tainted love|tampon|tard|taste my|tawdry|tea bagging|teabagging|teat|terd|teste|testee|testes|testicle|testis|threesome|throating|thrust|thug|tied up|tight white|tinkle|tit|titfuck|titi|tits|tittiefucker|titties|titty|tittyfuck|tittyfucker|toke|tongue in a|toots|topless|tosser|towelhead|tramp|tranny|transsexual|trashy|tribadism|tub girl|tubgirl|turd|tush|tushy|twat|twats|twink|twinkie|two girls one cup|ugly|undies|undressing|unwed|upskirt|urethra play|urinal|urine|urophilia|uterus|uzi|vag|vagina|valium|venus mound|viagra|vibrator|violet wand|virgin|vixen|vodka|vomit|vorarephilia|voyeur|vulgar|vulva|wad|wang|wank|wanker|wazoo|wedgie|weed|weenie|weewee|weiner|weirdo|wench|wet dream|wetback|wh0re|wh0reface|white power|whitey|whiz|whoralicious|whore|whorealicious|whored|whoreface|whorehopper|whorehouse|whores|whoring|wigger|womb|woody|wop|wrapping men|wrinkled starfish|wtf|x-rated|xx|xxx|yaoi|yeasty|yellow showers|yiffy|yobbo|zoophile|zoophilia|??",
    ptpOrderHistory: null,
    bannerOrderHistory: null,
    squareBannerOrderHistory: null,
    bonusAdOrderHistory: null,
    advertiseBonusAd: null,
    advertiseBonusAdTimeBased: null,
    adhitzStandardTextBannerHTMLCode:
        '<iframe src="http://faucet4all.com/Ads/AdhitzStandardText.html" scrolling="no" style="width:476px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>',
    adhitzStandardBannerHTMLCode:
        '<iframe src="http://faucet4all.com/Ads/AdhitzStandard.html" scrolling="no" style="width:476px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>',
    bonusAds: [],
    bonusAdsRecords: [],
    captchaModal: false,
    errorModal: false,
    successModal: false,
    messageModal: false,
    chatRulesModal: false,

    errorMessage: 'An error occured.',
    successMessage: null,
    messageModalMessage: null,

    //Reactive variables - These are binded directly to the dom to ensure they update upon data change

    reactiveBannerOrderData: [],
    reactivePTPOrderData: [],
    reactiveSquareBannerOrderData: [],
    reactiveBonusAdOrderData: []
};


export const store = new Vuex.Store({
    plugins: [pathify.plugin],
    state: stateData,
    mutations: make.mutations(stateData),
    getters: {
        faucetSettingValue: stateData => Name => {
            return (stateData.faucetSettings.find(instance => instance.Name === Name)).Value;
        },
        randomPPCBannerNetworkHTMLCode: stateData => {
            var randomNumber = 0;
            if (randomNumber === 0) return stateData.adhitzStandardTextBannerHTMLCode;
            else if (randomNumber === 1) return stateData.adhitzStandardBannerHTMLCode;
        }
    },
    strict: process.env.NODE_ENV !== 'production'
});

window.store = store;
