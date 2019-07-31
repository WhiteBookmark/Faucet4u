<template>
    <div v-html="skyscrapperBanner"></div>
</template>

<script>
    export default {
        name: "SkyscrapperBanner",
        data() {
            return {
                skyscrapperBanner: null
            };
        },
        async mounted() {

            try {
                const response = await this.axios.get(process.env.VUE_APP_API_SkyscraperBannerNetworkHTMLCode);
                this.skyscrapperBanner = response.data.HTMLCode;
            }
            catch (error) {
                console.log(error);
            }
        }
    };
</script>
