import Vue from "vue";
import Router from "vue-router";
import axios from "axios";
import VueCookie from "vue-cookie";

import Index from "@/components/Index/Index";
import Login from "@/components/Index/Login";
import AccountRegister from "@/components/Index/AccountRegister";
import EmailConfirm from "@/components/Index/EmailConfirm";
import ResendEmail from "@/components/Index/ResendEmail";
import PasswordReset from "@/components/Index/PasswordReset";
import PasswordResetCode from "@/components/Index/PasswordResetCode";
import SupportTickets from "@/components/Index/SupportTickets";
import ViewTickets from "@/components/Index/ViewTickets";
import SupportTicketsReply from "@/components/Index/SupportTicketsReply";
import Support from "@/components/Index/Support";
import TermsOfService from "@/components/Index/TermsOfService";
import PrivacyPolicy from "@/components/Index/PrivacyPolicy";
import Help from "@/components/Index/Help";

import Home from "@/components/Home/Index";
import Settings from "@/components/Home/Settings";
import Withdrawal from "@/components/Home/Withdrawal";

import PTP from "@/components/Home/PTP";
import LoginHistory from "@/components/Home/LoginHistory";
import WithdrawalHistory from "@/components/Home/WithdrawalHistory";
import Referrals from "@/components/Home/Referrals";
import Account from "@/components/Home/Account";
import Banners from "@/components/Home/Banners";
import HomeSupport from "@/components/Home/Support";
import HomeViewTickets from "@/components/Home/ViewTickets";
import HomeSupportTickets from "@/components/Home/SupportTickets";
import HomeSupportTicketsReply from "@/components/Home/SupportTicketsReply";
import Advertise from "@/components/Home/Advertise";
import OrderHistory from "@/components/Home/OrderHistory";
import Deposit from "@/components/Home/Deposit";
import DepositHistory from "@/components/Home/DepositHistory";
import Offerwalls from "@/components/Home/Offerwalls";
import Rates from "@/components/Home/Rates";
import AdvertiserPanelPTP from "@/components/Home/AdvertiserPanelPTP";
import AdvertiserPanelBanner from "@/components/Home/AdvertiserPanelBanner";
import AdvertiserPanelSquareBanner from "@/components/Home/AdvertiserPanelSquareBanner";
import AdvertiserPanelBonusAds from "@/components/Home/AdvertiserPanelBonusAds";
import BonusAds from "@/components/Home/BonusAds";

import Hitswall from "@/components/Offerwalls/Hitswall";
import PTCWall from "@/components/Offerwalls/PTCWall";
import SkippyAds from "@/components/Offerwalls/SkippyAds";
import KiwiWall from "@/components/Offerwalls/KiwiWall";
import OfferToro from "@/components/Offerwalls/OfferToro";

Vue.use(Router);

function requireAuth(to, from, next) {
    axios
        .get(process.env_APP_API_SessionValid, {
            params: { sessionId: VueCookie.get("sessionId") }
        })
        .then(() => {
            next();
        })
        .catch(() => {
            VueCookie.set("lsi", VueCookie.get("sessionId"), { expires: "30D" });
            VueCookie.delete("sessionId");
            store.set("authorization", 0);
            store.set("sessionValid", false);
            store.set("userData", null);
            next("/");
        });
}

const router = new Router({
    routes: [
        {
            path: "/",
            name: "Index",
            component: Index
        },
        {
            path: "/Login",
            name: "Login",
            component: Login
        },
        {
            path: "/AccountRegister",
            name: "AccountRegister",
            component: AccountRegister
        },
        {
            path: "/EmailConfirm",
            name: "EmailConfirm",
            component: EmailConfirm
        },
        {
            path: "/ResendEmail",
            name: "ResendEmail",
            component: ResendEmail
        },
        {
            path: "/PasswordReset",
            name: "PasswordReset",
            component: PasswordReset
        },
        {
            path: "/PasswordResetCode",
            name: "PasswordResetCode",
            component: PasswordResetCode
        },
        {
            path: "/SupportTickets",
            name: "SupportTickets",
            component: SupportTickets
        },
        {
            path: "/ViewTickets",
            name: "ViewTickets",
            component: ViewTickets
        },
        {
            path: "/SupportTicketsReply",
            name: "SupportTicketsReply",
            component: SupportTicketsReply
        },
        {
            path: "/Support",
            name: "Support",
            component: Support
        },
        {
            path: "/TermsOfService",
            name: "TermsOfService",
            component: TermsOfService
        },
        {
            path: "/PrivacyPolicy",
            name: "PrivacyPolicy",
            component: PrivacyPolicy
        },
        {
            path: "/Help",
            name: "Help",
            component: Help
        },
        //Routes from here require authentication of user logged in
        {
            path: "/Home",
            name: "Home",
            component: Home,
            beforeEnter: requireAuth
        },
        {
            path: "/Settings",
            name: "Settings",
            component: Settings,
            beforeEnter: requireAuth
        },
        {
            path: "/Withdrawal",
            name: "Withdrawal",
            component: Withdrawal,
            beforeEnter: requireAuth
        },
        {
            path: "/HomeSupportTickets",
            name: "HomeSupportTickets",
            component: HomeSupportTickets,
            beforeEnter: requireAuth
        },
        {
            path: "/HomeSupportTicketsReply",
            name: "HomeSupportTicketsReply",
            component: HomeSupportTicketsReply,
            beforeEnter: requireAuth
        },
        {
            path: "/PTP",
            name: "PTP",
            component: PTP,
            beforeEnter: requireAuth
        },
        {
            path: "/LoginHistory",
            name: "LoginHistory",
            component: LoginHistory,
            beforeEnter: requireAuth
        },
        {
            path: "/WithdrawalHistory",
            name: "WithdrawalHistory",
            component: WithdrawalHistory,
            beforeEnter: requireAuth
        },
        {
            path: "/Referrals",
            name: "Referrals",
            component: Referrals,
            beforeEnter: requireAuth
        },
        {
            path: "/Account",
            name: "Account",
            component: Account,
            beforeEnter: requireAuth
        },
        {
            path: "/Banners",
            name: "Banners",
            component: Banners,
            beforeEnter: requireAuth
        },
        {
            path: "/HomeSupport",
            name: "HomeSupport",
            component: HomeSupport,
            beforeEnter: requireAuth
        },
        {
            path: "/HomeViewTickets",
            name: "HomeViewTickets",
            component: HomeViewTickets,
            beforeEnter: requireAuth
        },
        {
            path: "/Advertise",
            name: "Advertise",
            component: Advertise,
            beforeEnter: requireAuth
        },
        {
            path: "/OrderHistory",
            name: "OrderHistory",
            component: OrderHistory,
            beforeEnter: requireAuth
        },
        {
            path: "/Deposit",
            name: "Deposit",
            component: Deposit,
            beforeEnter: requireAuth
        },
        {
            path: "/DepositHistory",
            name: "DepositHistory",
            component: DepositHistory,
            beforeEnter: requireAuth
        },
        {
            path: "/Offerwalls",
            name: "Offerwalls",
            component: Offerwalls,
            beforeEnter: requireAuth
        },
        {
            path: "/Hitswall",
            name: "Hitswall",
            component: Hitswall,
            beforeEnter: requireAuth
        },
        {
            path: "/PTCWall",
            name: "PTCWall",
            component: PTCWall,
            beforeEnter: requireAuth
        },
        {
            path: "/SkippyAds",
            name: "SkippyAds",
            component: SkippyAds,
            beforeEnter: requireAuth
        },
        {
            path: "/KiwiWall",
            name: "KiwiWall",
            component: KiwiWall,
            beforeEnter: requireAuth
        },
        {
            path: "/Rates",
            name: "Rates",
            component: Rates,
            beforeEnter: requireAuth
        },
        {
            path: "/AdvertiserPanelPTP",
            name: "AdvertiserPanelPTP",
            component: AdvertiserPanelPTP,
            beforeEnter: requireAuth
        },
        {
            path: "/AdvertiserPanelBanner",
            name: "AdvertiserPanelBanner",
            component: AdvertiserPanelBanner,
            beforeEnter: requireAuth
        },
        {
            path: "/AdvertiserPanelSquareBanner",
            name: "AdvertiserPanelSquareBanner",
            component: AdvertiserPanelSquareBanner,
            beforeEnter: requireAuth
        },
        {
            path: "/AdvertiserPanelBonusAds",
            name: "AdvertiserPanelBonusAds",
            component: AdvertiserPanelBonusAds,
            beforeEnter: requireAuth
        },
        {
            path: "/BonusAds",
            name: "BonusAds",
            component: BonusAds,
            beforeEnter: requireAuth
        },
        {
            path: "/OfferToro",
            name: "OfferToro",
            component: OfferToro,
            beforeEnter: requireAuth
        }
    ]
});

router.beforeResolve((to, from, next) => {
    store.set("isLoading", true);
    next();
});

router.afterEach((to, from) => {
    setTimeout(function () { store.set("isLoading", false); }, 2000);
});

export default router;
