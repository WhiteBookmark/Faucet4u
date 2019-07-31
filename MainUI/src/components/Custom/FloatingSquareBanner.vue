<template>
    <div v-if="posLeft">
        <button type="button" class="bottomLeftButton" @click="posLeft = !posLeft"> x </button>
        <div class="bottomLeftBanner">
            <square-banner></square-banner>
        </div>
    </div>
    <div v-else-if="posRight">
        <button type="button" class="bottomRightButton" @click="posRight = !posRight"> x </button>
        <div class="bottomRightBanner">
            <square-banner></square-banner>
        </div>
    </div>
    <div v-else></div>
</template>

<script>
    export default {
        name: "FloatingSquareBanner",
        props: {
            left: {
                type: Boolean,
                default: false
            },
            right: {
                type: Boolean,
                default: false
            }
        },
        data() {
            return {
                posLeft: this.left,
                posRight: this.right
            }
        },
    };
</script>
